using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shicitojson
{
    /// <summary>
    /// 诗人
    /// </summary>
    internal class Poet
    {
        private string mid;

        /// <summary>
        /// 诗人的_id 和小程序的记录的id映射对应上
        /// </summary>
        public string _id
        {
            get
            {
                if (string.IsNullOrEmpty(mid))
                {
                    mid = Guid.NewGuid().ToString().Replace("-", "");
                }
                return mid;
            }
            set { mid = value; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 朝代
        /// </summary>
        public string dynasty { get; set; }

        /// <summary>
        /// 出生时间
        /// </summary>
        public string birth { get; set; }

        /// <summary>
        /// 死亡时间
        /// </summary>
        public string death { get; set; }

        /// <summary>
        /// 诗歌id
        /// </summary>
        public List<string> poemIds { get; set; }=new List<string>();

    }
}

