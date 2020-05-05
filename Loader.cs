using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JSonParser
{
    public class Loader
    {
        public async Task LoadJson()
        {
            string jsonString = "";
            await Task.Run(async () =>
            {
                foreach (string item in SearchJson().Result)
                {
                    await Task.Run(() => jsonString = File.ReadAllTextAsync(item).Result);

                    Parser parser = new Parser();

                    await parser.ParseJson(jsonString, item);
                }
            });
        }

        private async Task<List<string>> SearchJson()
        {
            List<string> fileList = new List<string>();

#if (DEBUG==TRUE)
            string solutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            await Task.Run(() => fileList = Directory.GetFiles(solutionFolderPath + "//json//", "*.json").ToList());
#endif

#if (DEBUG==FALSE)
            await Task.Run(() => fileList = Directory.GetFiles(Directory.GetCurrentDirectory() + "//json//", "*.json").ToList()); 
#endif
            return fileList;
        }
    }
}
