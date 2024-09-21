using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace shici2dist
{
    /// <summary>
    /// 处理shici成json文件
    /// </summary>
    internal class Program
    {
        public static string libsPath = Path.Combine("..", "..", "..", "libs");
        public static string sourcePath = Path.Combine("..", "..", "..", "source");
        public static string textPath = Path.Combine(libsPath, "text");




        public static void Main(string[] args)
        {
            List<string> DynastyOrder = new List<string>
            {
                "先秦", "汉代", "三国两晋", "南北朝", "隋代", "唐代", "宋代", "元代", "明代", "清代", "近现代"
            };

            Console.WriteLine("Hello, World!");

            if (!Directory.Exists(sourcePath))
            {
                // 创建存放汇总信息的目录
                Directory.CreateDirectory(sourcePath);
            }

            Dictionary<string, Dynasty> dynasties = new Dictionary<string, Dynasty>();
            Dictionary<string, Poet> poets = new Dictionary<string, Poet>();

            string[] array = Directory.GetDirectories(textPath);
            for (int i = 0; i < array.Length; i++)
            {
                string? dynastyDir = array[i];
                var dynasty = new DirectoryInfo(dynastyDir).Name;
                if (!dynasties.ContainsKey(dynasty))
                {
                    dynasties[dynasty] = new Dynasty { Name = dynasty };
                }

                var dynastyOutputPath = Path.Combine(sourcePath, dynasty);
                Directory.CreateDirectory(dynastyOutputPath);

                string[] array1 = Directory.GetDirectories(dynastyDir);
                for (int i1 = 0; i1 < array1.Length; i1++)
                {
                    string? poetDir = array1[i1];
                    var poetName = new DirectoryInfo(poetDir).Name;
                    Poet poet;
                    if (!poets.ContainsKey(poetName))
                    {
                        poet = new Poet
                        {
                            Name = poetName,
                            Dynasty = dynasty,
                            Poems = new List<Poem>()
                        };

                        var metaPath = Path.Combine(poetDir, "meta.txt");
                        if (File.Exists(metaPath))
                        {
                            var authorInfo = File.ReadAllLines(metaPath);
                            poet.Birth = authorInfo.Length > 0 ? authorInfo[0].Replace("birth=", "") : "不详";
                            poet.Death = authorInfo.Length > 1 ? authorInfo[1].Replace("death=", "") : "不详";
                            poet.Description = authorInfo.Length >= 4 ? authorInfo[3] : "不详";
                        }

                        poets[poetName] = poet;
                        dynasties[dynasty].PoetNames.Add(poetName);
                    }
                    else
                    {
                        poet = poets[poetName];
                    }

                    foreach (var poemFile in Directory.GetFiles(poetDir).Where(f => !f.EndsWith("meta.txt")))
                    {
                        var poemName = Path.GetFileNameWithoutExtension(poemFile);
                        Poem poem = new Poem
                        {
                            Name = poemName,
                            PoetId = poet._id ?? (poet._id = Guid.NewGuid().ToString().Replace("-", "")),
                            PoetName = poetName,
                            Tags = new List<string> { dynasty, poetName },
                            Contents = new List<string>()
                        };

                        var poemInfo = File.ReadAllLines(poemFile);
                        if (poemInfo.Length > 0)
                        {
                            poem.Form = poemInfo[0].Replace("form=", "");
                        }
                        if (poemInfo.Length > 1 && poemInfo[1].StartsWith("tags="))
                        {
                            poem.Tags.AddRange(poemInfo[1].Replace("tags=", "").Split(',').Where(c => !string.IsNullOrEmpty(c)));
                        }
                        poem.Contents.AddRange(poemInfo.Skip(3).Where(c => !string.IsNullOrEmpty(c)));

                        poet.Poems.Add(poem);
                    }

                    // 生成诗人的JSON文件
                    string poetJsonFilePath = Path.Combine(dynastyOutputPath, $"{poetName}.json");
                    File.WriteAllText(poetJsonFilePath, JsonConvert.SerializeObject(poet, Formatting.Indented));
                }
            }

            // 按照 DynastyOrder 排序并生成汇总的JSON文件
            List<Dynasty> orderedDynasties = DynastyOrder
                .Where(dynasty => dynasties.ContainsKey(dynasty))
                .Select(dynasty => dynasties[dynasty])
                .ToList();

            string summaryJsonFilePath = Path.Combine(sourcePath, "dynasties.json");
            File.WriteAllText(summaryJsonFilePath, JsonConvert.SerializeObject(orderedDynasties, Formatting.Indented));

            Console.WriteLine("GoodBye, World!");
            Console.ReadLine();
        }
    }
}
