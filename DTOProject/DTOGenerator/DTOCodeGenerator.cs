using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;
using DTOProject.DTOGenerator.DTOObjects;
using DTOProject.DTOGenerator.DTOTypes;
using DTOProject.DTOGenerator.CSCodeContainer;
using DTOProject.DTOGenerator.DTOToTranslaters;

namespace DTOProject.DTOGenerator
{
    public class DTOCodeGenerator
    {
        private SyntaxList<CompilationUnitSyntax> unitDeclarations;
        private CountdownEvent countDownEvent;
        private AutoResetEvent poolLimitEvent;
        private volatile int currentThreadNumber;
        private static object locker = new object();
        private string unitNamespace;
        private int poolLimit;
        private int completionPortThreads;
        private ILogger logger;

        public DTOCodeGenerator(int poolLimit, string unitNamespace, ILogger logger)
        {
            this.logger = logger;
            this.unitNamespace = unitNamespace;
            SetThreadPoolLimits(poolLimit);
            unitDeclarations = new SyntaxList<CompilationUnitSyntax>();
        }

        public CodeContainer Generate(DTOUnitDescription unitDescription)
        {
            if (CheckUnitDescription(unitDescription))
            {
                DTOToCSCodeTranslater codeTranslater = new DTOToCSCodeTranslater();

                return codeTranslater.Translate(GenerateUnitNamespace(unitDescription), unitDescription);
            }
            else
            {
                return null;
            }
        }

        private void ThreadTasksDispetcher(object state)
        {
            SyntaxList<MemberDeclarationSyntax> classDeclarationsList = new SyntaxList<MemberDeclarationSyntax>();
            classDeclarationsList = GenerateClassCode((DTOClassDescription)state);
            CompilationUnitSyntax unitDeclaration = CompilationUnit()
                                .WithUsings(
                                SingletonList<UsingDirectiveSyntax>(
                                    UsingDirective(
                                        IdentifierName("System"))))
                                    .WithMembers(
                                        SingletonList<MemberDeclarationSyntax>(
                                            NamespaceDeclaration(
                                                IdentifierName(unitNamespace))
                                            .WithMembers(classDeclarationsList)))
                                .NormalizeWhitespace();
            lock (locker)
            {
                unitDeclarations = unitDeclarations.Add(unitDeclaration);
            }

            Interlocked.Decrement(ref currentThreadNumber);
            poolLimitEvent.Set();

            countDownEvent.Signal();
            
        }

        private SyntaxList<CompilationUnitSyntax> GenerateUnitNamespace(DTOUnitDescription unitDescription)
        {
            countDownEvent = new CountdownEvent(unitDescription.ClassDescriptions.Count);
            //ThreadPool.SetMaxThreads(poolLimit, completionPortThreads);
            currentThreadNumber = 0;
            poolLimitEvent = new AutoResetEvent(true);

            foreach (DTOClassDescription classDescription in unitDescription.ClassDescriptions)
            {
                poolLimitEvent.WaitOne();
                Interlocked.Increment(ref currentThreadNumber);
                ThreadPool.QueueUserWorkItem(ThreadTasksDispetcher, classDescription);
                if (currentThreadNumber == poolLimit)
                {
                    poolLimitEvent.Reset();
                }
                else
                {
                    poolLimitEvent.Set();
                }
            }

            countDownEvent.Wait();

            return unitDeclarations;
        }

        private SyntaxList<MemberDeclarationSyntax> GenerateClassCode(DTOClassDescription classDescription)
        {
            SyntaxList<MemberDeclarationSyntax> memberDeclarationsList = new SyntaxList<MemberDeclarationSyntax>();

            foreach (DTOMemberDescription memberDescription in classDescription.MemberDescriptions)
            {
                MemberDeclarationSyntax memberDeclaration = GenerateMemberCode(memberDescription);
                if (memberDeclaration != null)
                {
                    memberDeclarationsList = memberDeclarationsList.Add(memberDeclaration);
                }
            }

            ClassDeclarationSyntax classDeclaration = ClassDeclaration(classDescription.ClassName)
                            .WithModifiers(
                                TokenList(
                                Token(SyntaxKind.PublicKeyword)))
                            .WithMembers(memberDeclarationsList);

            SyntaxList<MemberDeclarationSyntax> resultClassDeclaration = new SyntaxList<MemberDeclarationSyntax>();
            resultClassDeclaration = resultClassDeclaration.Add(classDeclaration);

            return resultClassDeclaration;
        }

        private MemberDeclarationSyntax GenerateMemberCode(DTOMemberDescription memberDescription)
        {
            TypesStorage typesStorage = new TypesStorage();

            try
            {
                string csType = typesStorage.GetCSType(memberDescription.Type, memberDescription.Format);
                MemberDeclarationSyntax memberDeclaration = PropertyDeclaration(
                                                        IdentifierName(csType),
                                                        Identifier(memberDescription.Name))
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(SyntaxKind.PublicKeyword)))
                                                    .WithAccessorList(
                                                        AccessorList(
                                                            List<AccessorDeclarationSyntax>(
                                                                new AccessorDeclarationSyntax[]{
                                                                    AccessorDeclaration(
                                                                        SyntaxKind.GetAccessorDeclaration)
                                                                    .WithSemicolonToken(
                                                                        Token(SyntaxKind.SemicolonToken)),
                                                                    AccessorDeclaration(
                                                                        SyntaxKind.SetAccessorDeclaration)
                                                                    .WithModifiers(
                                                                        TokenList(
                                                                            Token(SyntaxKind.PrivateKeyword)))
                                                                    .WithSemicolonToken(
                                                                        Token(SyntaxKind.SemicolonToken))})));

                return memberDeclaration;

            }
            catch(ArgumentException e)
            {
                logger.Log(e.Message);
                return null;
            }
        }

        private bool CheckUnitDescription(DTOUnitDescription unitDescription)
        {
            try
            {
                foreach (DTOClassDescription classDescription in unitDescription.ClassDescriptions)
                {
                    if (classDescription.ClassName.Equals(""))
                    {
                        return false;
                    }
                    foreach (DTOMemberDescription memberDescription in classDescription.MemberDescriptions)
                    {
                        if ((memberDescription.Name.Equals("")) || (memberDescription.Format.Equals(""))
                            || (memberDescription.Type.Equals("")))
                        {
                            return false;
                        }
                    }
                }
            }
            catch(NullReferenceException e)
            {
                logger.Log(e.Message);
                return false;
            }

            return true;
        }

        private void SetThreadPoolLimits(int poolLimit)
        {
            int workerThreads;
            int completionPortThreads;
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);

            if(poolLimit < workerThreads)
            {
                this.poolLimit = workerThreads;
            }
            else
            {
                this.poolLimit = poolLimit;
            }

            this.completionPortThreads = completionPortThreads;
        }
    }
}
