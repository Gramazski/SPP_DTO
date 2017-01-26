using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOProject.DTOGenerator.DTOObjects
{
    public class DTOUnitDescription
    {
        public List<DTOClassDescription> ClassDescriptions { get; set; }

        public DTOUnitDescription()
        {
            ClassDescriptions = new List<DTOClassDescription>();
        }
    }
}
