using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOClasses;

namespace IOClasses
{
    class MultiFileWriter
    {
        public void WriteToFiles(List<String> fileNames, List<String> fileContents ,string directoryPath)
        {
            FileWriter fileWriter = new FileWriter();

            if ((fileNames == null) && (fileContents == null))
            {
                throw new ArgumentException("File names and file content must be not null!");
            }

            try
            {
                for (int i = 0; i < fileNames.Count; i++)
                {
                    fileWriter.WriteToFile(fileNames[i], directoryPath, fileContents[i]);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Multi writer" + e.Message);
            }

        }
    }
}
