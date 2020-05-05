using Newtonsoft.Json.Linq;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JSonParser
{
    public class Parser
    {
        public async Task ParseJson(string jsonString, string path)
        {
            dynamic json = JArray.Parse(jsonString);
            string text_entry = "";
            string searchEntry = "text_entry";
            await Task.Run(async () =>
            {
                foreach (JObject item in json)
                {
                    // item is body containing "index","type", "line_id", "play_name", "speech_number", "speaker", "text_entry"
                    try
                    {
                        if (!item.ContainsKey(searchEntry))
                        {
                            continue;
                        }

                        text_entry = item[searchEntry].ToString();

                        if (text_entry.StartsWith("ACT") || text_entry.StartsWith("SCENE") || text_entry.StartsWith("Exeunt") 
                        || text_entry.StartsWith("[Reads]") || text_entry.StartsWith("[Aside]"))
                        {
                            continue;
                        }

                        if (text_entry.Contains('.'))
                        {
                            text_entry = text_entry.Replace(".", "." + Environment.NewLine);
                            text_entry = text_entry.Replace(". ", "." + Environment.NewLine);
                        }
                        
                        if (!text_entry.Contains('.'))
                        {
                            text_entry += ' ';
                        }

                        Console.WriteLine(text_entry);
                        await File.AppendAllTextAsync(path + ".txt", text_entry);

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Skipping line because " + ex.Message);
                        continue;
                    }
                }
            });
        }
    }
}
