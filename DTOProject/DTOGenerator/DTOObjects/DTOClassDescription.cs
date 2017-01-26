using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOProject.DTOGenerator.DTOObjects
{
    public class DTOClassDescription
    {
        public string ClassName { get; set; }
        public List<DTOMemberDescription> MemberDescriptions { get; set; }
    }
}
