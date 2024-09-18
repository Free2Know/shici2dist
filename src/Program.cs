using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace shicitojson
{
    /// <summary>
    /// 处理shici成json文件
    /// </summary>
    internal class Program
    {
        public static string libsPath = Path.Combine("..", "..", "..", "libs");
        public static string distPath = Path.Combine("..", "..", "..", "dist");
        public static string textPath = Path.Combine(libsPath, "text");
        public static string poetJsonPath = Path.Combine(distPath, "poets");
        public static string poemJsonPath = Path.Combine(distPath, "poems");

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // 创建存放诗人和诗词的目录
            Directory.CreateDirectory(poetJsonPath);
            Directory.CreateDirectory(poemJsonPath);

            foreach (var dynastyDir in Directory.GetDirectories(textPath))
            {
                var dynasty = new DirectoryInfo(dynastyDir).Name;
                foreach (var poetDir in Directory.GetDirectories(dynastyDir))
                {
                    var poetName = new DirectoryInfo(poetDir).Name;
                    Poet poet = new Poet
                    {
                        name = poetName,
                        dynasty = dynasty,
                        poemIds = new List<string>()
                    };

                    var metaPath = Path.Combine(poetDir, "meta.txt");
                    if (File.Exists(metaPath))
                    {
                        var authorInfo = File.ReadAllLines(metaPath);
                        poet.birth = authorInfo.Length > 0 ? authorInfo[0].Replace("birth=", "") : "不详";
                        poet.death = authorInfo.Length > 1 ? authorInfo[1].Replace("death=", "") : "不详";
                        poet.description = authorInfo.Length >= 4 ? authorInfo[3] : "不详";
                    }

                    foreach (var poemFile in Directory.GetFiles(poetDir).Where(f => !f.EndsWith("meta.txt")))
                    {
                        var poemName = Path.GetFileNameWithoutExtension(poemFile);
                        Poem poem = new Poem
                        {
                            name = poemName,
                            poetId = poet._id, // 假设Poet类中有一个_id属性用于唯一标识诗人
                            poetName = poetName,
                            tags = new List<string> { dynasty, poetName },
                            contents = new List<string>()
                        };

                        var poemInfo = File.ReadAllLines(poemFile);
                        if (poemInfo.Length > 0)
                        {
                            poem.form = poemInfo[0].Replace("form=", "");
                        }
                        else
                        {
                            poem.form = ""; // 或者根据业务逻辑设置默认值
                        }
                        if (poemInfo.Length > 1 && poemInfo[1].StartsWith("tags="))
                        {
                            poem.tags.AddRange(poemInfo[1].Replace("tags=", "").Split(','));
                        }
                        poem.contents.AddRange(poemInfo.Skip(3).Where(c => !string.IsNullOrEmpty(c)));

                        // 检查诗歌JSON文件是否已存在
                        string poemJsonFilePath = Path.Combine(poemJsonPath, $"{poemName}.json");
                        if (!File.Exists(poemJsonFilePath))
                        {
                            File.WriteAllText(poemJsonFilePath, JsonConvert.SerializeObject(poem, Formatting.Indented));
                        }
                        else
                        {
                            Console.WriteLine($"Skipped writing poem {poemName}, file already exists.");
                        }

                        poet.poemIds.Add(poem._id);
                    }

                    // 检查诗人JSON文件是否已存在
                    string poetJsonFilePath = Path.Combine(poetJsonPath, $"{poetName}.json");
                    if (!File.Exists(poetJsonFilePath))
                    {
                        File.WriteAllText(poetJsonFilePath, JsonConvert.SerializeObject(poet, Formatting.Indented));
                    }
                    else
                    {
                        Console.WriteLine($"Skipped writing poet {poetName}, file already exists.");
                    }
                }
            }

            Console.WriteLine("GoodBye, World!");
            Console.ReadLine();
        }

    }
}