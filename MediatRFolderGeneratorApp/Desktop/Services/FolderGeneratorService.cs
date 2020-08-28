using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Desktop.Models;

namespace Desktop.Services
{
    public static class FolderGeneratorService
    {
        public async static Task GenerateFolders(List<StructureEntity> list, string rootPath, string rootNamespace)
        {
            foreach (var structure in list)
            {
                //create folder
                var folderPath = Path.Combine(rootPath, $"{structure.Before}{structure.After}");
                Directory.CreateDirectory(folderPath);

                //Get Text from templates
                var templateRootPath = $"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}\\Templates";

                var requestText = File.ReadAllText($"{templateRootPath}\\RequestTemplate.txt");
                var handlerText = File.ReadAllText($"{templateRootPath}\\HandlerTemplate.txt");
                var validatorText = File.ReadAllText($"{templateRootPath}\\ValidatorTemplate.txt");



                //create namespace
                var nameSpace = $"{rootNamespace}.{structure.Before}{structure.After}";

                //create request name
                var name = (string.IsNullOrWhiteSpace(structure.CustomRootName)) ? structure.RootName : structure.CustomRootName;
                var requestName = $"{structure.Before}{name}{structure.After}{structure.MediatrType}";

                //create handler name
                var handlerName = $"{structure.Before}{name}{structure.After}Handler";

                //create validator name
                var validatorName = $"{structure.Before}{name}{structure.After}Validator";


                //replace text
                requestText = requestText.Replace("{NAMESPACENAME}", nameSpace);
                requestText = requestText.Replace("{REQUESTNAME}", requestName);

                handlerText = handlerText.Replace("{NAMESPACENAME}", nameSpace);
                handlerText = handlerText.Replace("{REQUESTNAME}", requestName);
                handlerText = handlerText.Replace("{HANDLERNAME}", handlerName);

                validatorText = validatorText.Replace("{NAMESPACENAME}", nameSpace);
                validatorText = validatorText.Replace("{VALIDATORNAME}", validatorName);
                validatorText = validatorText.Replace("{REQUESTNAME}", requestName);


                //create files
                using (StreamWriter sw = File.CreateText($"{folderPath}\\{requestName}.cs"))
                {
                    await sw.WriteAsync(requestText);
                }
                using (StreamWriter sw = File.CreateText($"{folderPath}\\{handlerName}.cs"))
                {
                    await sw.WriteAsync(handlerText);
                }

                if (structure.IncludeValidator)
                {
                    using (StreamWriter sw = File.CreateText($"{folderPath}\\{validatorName}.cs"))
                    {
                        await sw.WriteAsync(validatorText);
                    }
                }
            }
        }
    }
}