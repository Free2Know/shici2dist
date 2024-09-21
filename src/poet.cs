using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shici2dist
{
    /// <summary>
    /// 诗人
    /// </summary>
    internal class Poet
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dynasty { get; set; }
        public string Birth { get; set; }
        public string Death { get; set; }
        public List<Poem> Poems { get; set; } = new List<Poem>();
    }
}

