using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shici2dist
{
    /// <summary>
    /// 诗歌
    /// </summary>

    internal class Poem
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string PoetId { get; set; }
        public string PoetName { get; set; }
        public string Form { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Contents { get; set; }
    }
}
