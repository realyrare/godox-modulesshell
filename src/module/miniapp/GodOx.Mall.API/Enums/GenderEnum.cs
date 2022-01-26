using System.ComponentModel;

/*************************************
* 类名：GenderEnum
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/10 19:57:30
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Enums
{
    public enum GenderEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Man = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Woman女 = 2
    }
}