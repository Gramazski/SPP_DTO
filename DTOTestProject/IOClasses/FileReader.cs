using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IOClasses
{
    class FileReader
    {
        public string ReadFromFile(string filePath)
        {
            string data = null;

            try
            {
                data = File.ReadAllText(filePath);
            }
            catch(Exception e)
            {
                throw new ArgumentException("Read from file: " + e.Message);
            }

            return data;
        }
    }
}
