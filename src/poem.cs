using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shicitojson
{
    /// <summary>
    /// 诗歌
    /// </summary>
    internal class Poem
    {
        private string mid;
        /// <summary>
        /// 诗人的_id 和FreeToKnow小程序的记录的_id映射对应上
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
        /// 名称
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// 正文内容
        /// </summary>
        public List<string> contents { get; set; }

        /// <summary>
        /// 诗歌类型
        /// </summary>
        public string ptype { get; set; }

        /// <summary>
        /// 诗人名称
        /// </summary>
        public string poetName { get; set; }


        /// <summary>
        /// 诗人主键
        /// </summary>
        public string poetId { get; set; }
        public string form { get; internal set; }
        public List<string> tags { get; internal set; }
    }
}
