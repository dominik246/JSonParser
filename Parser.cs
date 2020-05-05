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

                        if (text_entry.StartsWith("ACT") || text_entry.StartsWith("SCENE"))
                        {
                            continue;
                        }

                        if (text_entry.Contains('.'))
                        {
                            if(!text_entry.Contains(". . .") && item["speech_number"].ToString() != "196")
                            {
                                text_entry = text_entry.Replace(". ", ".");
                            }
                            text_entry = text_entry.Replace(".", "." + Environment.NewLine);
                        }
                        
                        while (text_entry.StartsWith(' '))
                        {
                            text_entry = text_entry.Remove(0, 1);
                        }

                        if (text_entry.Contains("[Reads]"))
                        {
                            text_entry = text_entry.Replace("[Reads] ", "");
                        }

                        if (text_entry.Contains("[Aside]"))
                        {
                            text_entry = text_entry.Replace("[Aside] ", "");
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
