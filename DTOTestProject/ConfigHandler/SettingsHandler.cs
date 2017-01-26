using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DTOTestProject.ConfigHandler
{
    class SettingsHandler : ISettingsHandler
    {
        public int GetThreadPoolLimit()
        {
            int poolLimit;

            if ((ConfigurationManager.AppSettings.Get("poolLimit") == null) || 
                !(Int32.TryParse(ConfigurationManager.AppSettings.Get("poolLimit"), out poolLimit)))
            {
                throw new ArgumentException("Wrong pool limit.");
            }

            return poolLimit;
        }

        public string GetDtoNamespace()
        {
            string DTONamespace = ConfigurationManager.AppSettings.Get("DTONamespace");
            if ((DTONamespace == null) || (DTONamespace == string.Empty))
            {
                throw new ArgumentException("Wrong DTO namespace.");
            }

            return DTONamespace;
        }
    }
}
