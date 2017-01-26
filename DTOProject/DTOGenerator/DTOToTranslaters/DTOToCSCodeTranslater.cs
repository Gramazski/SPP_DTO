using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOProject.DTOGenerator.CSCodeContainer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;
using DTOProject.DTOGenerator.DTOObjects;
using DTOProject.DTOGenerator.DTOTypes;

namespace DTOProject.DTOGenerator.DTOToTranslaters
{
    class DTOToCSCodeTranslater
    {
        public CodeContainer Translate(SyntaxList<CompilationUnitSyntax> unitDescriptions, 
            DTOUnitDescription unitDescription)
        {
            DTOToStringTranslater stringTranslater = new DTOToStringTranslater();

            CodeContainer codeContainer = new CodeContainer(GenerateClassNamesList(unitDescription), 
                stringTranslater.Translate(unitDescriptions));

            return codeContainer;
        }

        private List<String> GenerateClassNamesList(DTOUnitDescription unitDescription)
        {
            List<String> classNames = new List<string>();

            foreach(DTOClassDescription classDescription in unitDescription.ClassDescriptions)
            {
                classNames.Add(classDescription.ClassName);
            }

            return classNames;
        }
    }
}
