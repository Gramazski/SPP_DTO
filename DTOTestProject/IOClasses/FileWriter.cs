using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IOClasses
{
    class FileWriter
    {
        public void WriteToFile(string fileName, string directoryPath, string data)
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, fileName + ".cs");
                File.WriteAllText(filePath, data);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Write to file: " + e.Message);
            }
        }
    }
}
