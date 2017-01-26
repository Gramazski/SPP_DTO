using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;
using DTOProject.DTOGenerator.DTOObjects;
using DTOProject.DTOGenerator.DTOTypes;
using DTOProject.DTOGenerator.CSCodeContainer;

namespace DTOProject.DTOGenerator.CSCodeContainer
{
    class DTOToStringTranslater
    {
        public List<String> Translate(SyntaxList<CompilationUnitSyntax> unitDescriptions)
        {
            List<String> codeList = new List<String>();

            foreach(CompilationUnitSyntax unitDescription in unitDescriptions)
            {
                codeList.Add(unitDescription.ToFullString());
            }

            return codeList;
        }
    }
}
