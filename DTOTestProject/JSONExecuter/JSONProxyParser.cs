using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOProject.DTOGenerator.DTOObjects;
using IOClasses;

namespace DTOProject.JSONExecuter
{
    class JSONProxyParser
    {
        private ILogger logger;

        public JSONProxyParser(ILogger logger)
        {
            this.logger = logger;
        }

        public DTOUnitDescription GetDataFromFile(string filePath)
        {
            JSONParser jsonParser = new JSONParser();
            FileReader fileReader = new FileReader();

            try
            {
                return jsonParser.Parse(fileReader.ReadFromFile(filePath));
            }
            catch(ArgumentException e)
            {
                logger.Log(e.Message);
                return new DTOUnitDescription();
            }
            
        }
    }
}
