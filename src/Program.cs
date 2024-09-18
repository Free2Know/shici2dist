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
                    Poet poet = new Poet();
                    poet.name = poetName;
                    poet.dynasty = dynasty;

                    var metaPath = Path.Combine(poetDir, "meta.txt");
                    if (File.Exists(metaPath))
                    {
                        var authorInfo = File.ReadAllLines(metaPath);
                        poet.birth = authorInfo.Length > 0 ? authorInfo[0].Replace("birth=", "") : "不详";
                        poet.death = authorInfo.Length > 1 ? authorInfo[1].Replace("death=", "") : "不详";
                        poet.description = authorInfo.Length >= 4 ? authorInfo[3] : "不详";
                    }

                    poet.poemIds = new List<string>();
                    foreach (var poemFile in Directory.GetFiles(poetDir).Where(f => !f.EndsWith("meta.txt")))
                    {
                        var poemName = Path.GetFileNameWithoutExtension(poemFile);
                        Poem poem = new Poem();
                        poem.name = poemName;
                        poem.poetId = poet._id;
                        poem.poetName = poetName;

                        var poemInfo = File.ReadAllLines(poemFile);
                        if (poemInfo.Length > 0)
                        {
                            poem.form = poemInfo[0].Replace("form=", "");
                        }
                        else
                        {
                            poem.form = ""; // 或者根据业务逻辑设置默认值
                        }
                        poem.tags = new List<string> { dynasty, poetName };
                        if (poemInfo.Length > 1 && poemInfo[1].StartsWith("tags="))
                        {
                            poem.tags.AddRange(poemInfo[1].Replace("tags=", "").Split(','));
                        }
                        poem.contents = poemInfo.Skip(3).Where(c => !string.IsNullOrEmpty(c)).ToList();

                        // 存储诗歌JSON
                        string poemJsonFilePath = Path.Combine(poemJsonPath, $"{poemName}.json");
                        File.WriteAllText(poemJsonFilePath, JsonConvert.SerializeObject(poem, Formatting.Indented));

                        poet.poemIds.Add(poem._id);
                    }

                    // 存储诗人JSON
                    string poetJsonFilePath = Path.Combine(poetJsonPath, $"{poetName}.json");
                    File.WriteAllText(poetJsonFilePath, JsonConvert.SerializeObject(poet, Formatting.Indented));
                }
            }

            Console.WriteLine("GoodBye, World!");
            Console.ReadLine();
        }
    }


}