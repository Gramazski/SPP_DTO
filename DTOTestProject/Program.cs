using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DTOProject.DTOGenerator;
using DTOProject.JSONExecuter;
using IOClasses;
using DTOTestProject.ConfigHandler;
using DTOTestProject.Logger;
using DTOProject;
using DTOProject.DTOGenerator.CSCodeContainer;

namespace DTOTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxySettingsHandler settingsHandler = new ProxySettingsHandler(new ConsoleLogger());
            int DTOPoolLimit = settingsHandler.GetThreadPoolLimit();
            string DTONamespace = settingsHandler.GetDtoNamespace();

            DTOCodeGenerator codeGenerator = new DTOCodeGenerator(DTOPoolLimit, DTONamespace, new ConsoleLogger());
            JSONProxyParser jsonParser = new JSONProxyParser(new ConsoleLogger());
            CodeContainer generatedCode = codeGenerator.Generate(jsonParser.GetDataFromFile(GetFilePathFromConsole()));

            if (generatedCode != null)
            {
                MultiFileWriter multiFileWriter = new MultiFileWriter();
                ILogger logger = new ConsoleLogger();

                try
                {
                    multiFileWriter.WriteToFiles(generatedCode.Name,
                    generatedCode.Code, GetOutputDirectoryFromConsole());
                    Console.WriteLine("All right!");
                }
                catch(ArgumentException e)
                {
                    logger.Log(e.Message);
                }
                
            }
            else
            {
                Console.WriteLine("Can not generate cs code!");
            }

            Console.ReadLine();

        }

        private static string GetFilePathFromConsole()
        {
            string filePath = string.Empty;
            bool pathIsCorrect = false;

            while (!pathIsCorrect)
            {
                Console.WriteLine("Enter JSON file path:");
                filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File doesn't exists. Try again.");
                }
                else
                {
                    pathIsCorrect = true;
                }
            }

            return filePath;
        }

        private static string GetOutputDirectoryFromConsole()
        {
            string outputDirectory = string.Empty;
            Console.WriteLine("Enter output directory path:");
            outputDirectory = Console.ReadLine();
            return outputDirectory;
        }
    }
}
