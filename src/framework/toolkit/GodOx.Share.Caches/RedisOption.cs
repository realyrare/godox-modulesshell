/*************************************
* 类名：RedisOption
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/25 18:52:47
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Share.Caches
{
    /// <summary>
    /// redis配置信息实体
    /// </summary>
    public class RedisOption
    {
        public bool Enable { get; set; }
        public string Connection { get; set; }
        public string InstanceName { get; set; }
        public int Database { get; set; }
    }
}