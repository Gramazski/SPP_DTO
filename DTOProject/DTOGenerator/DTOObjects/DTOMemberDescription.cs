using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOProject.DTOGenerator.DTOObjects
{
    public class DTOMemberDescription
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }

        public DTOMemberDescription(string name, string type, string format)
        {
            Name = name;
            Type = type;
            Format = format;
        }
    }
}
