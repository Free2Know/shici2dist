
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
        public static string libsPath = @"..\..\..\libs";
        public static string distPath = @"..\..\..\dist";
        public static string textPath = Path.Combine(libsPath, "text");
        public static string poemPath = Path.Combine(distPath, "poem.json");
        public static string poetPath = Path.Combine(distPath, "poet.json");
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            List<Poem> poemlist = new List<Poem>(); //诗歌集合
            List<Poet> poetlist = new List<Poet>(); //诗歌集合

            foreach (var item in Directory.GetDirectories(textPath))
            {
                FileInfo fi = new FileInfo(item);
                var dynasty = fi.Name.Split(".").Last();
                foreach (var poets in Directory.GetDirectories(item))
                {
                    FileInfo poetFile = new FileInfo(poets);
                    Poet poet = new Poet();
                    poet.name = poetFile.Name;
                    poet.dynasty = dynasty;
                    var poetName = poetFile.Name;
                    foreach (var file in Directory.GetFiles(poets))
                    {
                        FileInfo poemFile = new FileInfo(file);

                        var poemName = Path.GetFileNameWithoutExtension(poemFile.FullName);
                        if (poemFile.Name == "meta.txt")
                        {
                            var authorInfo = File.ReadAllLines(file);//作者信息
                            var birthValue = authorInfo[0].Replace("birth=", "");
                            if (!string.IsNullOrEmpty(birthValue))
                            {
                                poet.birth = birthValue;

                            }
                            else
                            {
                                poet.birth = "不详";
                            }
                            var deathValue = authorInfo[1].Replace("death=", "");
                            if (!string.IsNullOrEmpty(deathValue))
                            {
                                poet.death = deathValue;

                            }
                            else
                            {
                                poet.death = "不详";
                            }
                            if (authorInfo.Length >= 4)
                            {
                                var desc = authorInfo[3];
                                if (!string.IsNullOrEmpty(desc))
                                {
                                    poet.description = desc;

                                }
                                else
                                {
                                    poet.description = "不详";
                                }
                            }


                        }
                        else
                        {
                            Poem poem = new Poem();
                            poem.name = poemName;
                            poem.poetId = poet._id;
                            poem.poetName = poetName;

                            var poemInfo = File.ReadAllLines(file); //作者信息
                            var formValue = poemInfo[0].Replace("form=", "");
                            if (!string.IsNullOrEmpty(formValue))
                            {
                                poem.form = formValue;

                            }

                            var tagValue = poemInfo[1].Replace("tags=", "");
                            if (!string.IsNullOrEmpty(tagValue))
                            {
                                poem.tags = new List<string> { dynasty, poetName, tagValue };

                            }
                            else
                            {
                                poem.tags = new List<string> { dynasty, poetName };
                            }
                            poem.contents = poemInfo.Skip(3).Where(c => !string.IsNullOrEmpty(c)).ToList();

                            //Console.WriteLine($"朝代 {dynasty} 诗人{poetName} 诗名  {poemName}"); //fi.Name  朝代
                            poemlist.Add(poem);
                            poet.poemIds.Add(poem._id);
                        }

                    }

                    poetlist.Add(poet);
                }
            }
            File.WriteAllLines(poetPath, poetlist.Select(c => JsonConvert.SerializeObject(c)));

            File.WriteAllLines(poemPath, poemlist.Select(c => JsonConvert.SerializeObject(c)));
            Console.WriteLine("GoodBye, World!");
            Console.ReadLine();
        }
    }
}

