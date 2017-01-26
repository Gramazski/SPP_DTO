using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOProject.DTOGenerator.DTOObjects;
using Newtonsoft.Json;

namespace DTOProject.JSONExecuter
{
    class JSONParser
    {
        public DTOUnitDescription Parse(string jsonString)
        {
            DTOUnitDescription unitDescription = new DTOUnitDescription();

            try
            {
                unitDescription = JsonConvert.DeserializeObject<DTOUnitDescription>(jsonString);
            }
            catch(JsonException e)
            {
                throw new ArgumentException("Parse json data: " + e.Message);
            }

            return unitDescription;
        }
    }
}
