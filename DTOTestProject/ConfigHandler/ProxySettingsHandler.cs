using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOProject;

namespace DTOTestProject.ConfigHandler
{
    class ProxySettingsHandler : ISettingsHandler
    {
        private ISettingsHandler settingsHandler;
        private ILogger logger;
        private const int defaultPoolLimit = 4;
        private const string defaultNamespace = "DTONamespace";

        public ProxySettingsHandler(ILogger logger)
        {
            settingsHandler = new SettingsHandler();
            this.logger = logger;
        }

        public string GetDtoNamespace()
        {
            try
            {
                return settingsHandler.GetDtoNamespace();
            }
            catch (ArgumentException e)
            {
                logger.Log(e.Message + "Was set: " + defaultNamespace);
                return defaultNamespace;
            }
        }

        public int GetThreadPoolLimit()
        {
            try
            {
                return settingsHandler.GetThreadPoolLimit();
            }
            catch (ArgumentException e)
            {
                logger.Log(e.Message + "Was set: " + defaultPoolLimit);
                return defaultPoolLimit;
            }
        }
    }
}
