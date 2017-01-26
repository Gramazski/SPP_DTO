using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOTestProject.ConfigHandler
{
    public interface ISettingsHandler
    {
        int GetThreadPoolLimit();
        string GetDtoNamespace();
    }
}
