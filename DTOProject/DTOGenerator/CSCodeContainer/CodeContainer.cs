using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOProject.DTOGenerator.CSCodeContainer
{
    public class CodeContainer
    {
        public List<string> Name { get; private set; }
        public List<string> Code { get; private set; }

        public CodeContainer(List<string> name, List<string> code)
        {
            Name = name;
            Code = code;
        }

    }
}
