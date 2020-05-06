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

                        if (text_entry.Contains("[Reads]"))
                        {
                            text_entry = text_entry.Replace("[Reads] ", "");
                        }

                        if (text_entry.Contains("[Aside]"))
                        {
                            text_entry = text_entry.Replace("[Aside] ", "");
                        }
                        int count = text_entry.Length;
                        //Console.WriteLine(text_entry);
                        //Console.WriteLine("-------");
                        for (int i = 0; i < count; i++)
                        {
                            // Checks if <index> is bigger than the string length
                            if (i == count - 1)
                            {
                                break;
                            }

                            //Console.Write(text_entry[i]);
                            char c = text_entry[i];

                            // Checks if there are two different case characters like eC or aE or iV and separates them
                            if (char.IsLower(c) && char.IsUpper(text_entry[i + 1]))
                            {
                                if (char.IsUpper(text_entry[i + 1]))
                                    continue;
                                text_entry = text_entry.Replace(text_entry[i].ToString(), text_entry[i] + " ");
                                count++;
                            }

                            // Checks if .:?!;, are together with a letter or a number
                            if ((c == ':' || c == '?' || c == '!' || c == ';' || c == ',') && char.IsLetterOrDigit(text_entry[i + 1]))
                            {
                                text_entry = text_entry.Replace(text_entry[i].ToString(), text_entry[i] + " ");
                                count++;
                            }
                        }

                        if (!text_entry.EndsWith(' ') && !text_entry.EndsWith(Environment.NewLine))
                        {
                            text_entry += ' ';
                        }

                        while (char.IsWhiteSpace(text_entry[0]))
                        {
                            text_entry = text_entry.Remove(0, 1);
                        }

                        //Console.Write(text_entry);
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
