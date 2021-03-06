using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

/*************************************
* 类名：Keyword
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/31 19:03:29
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Models.Entity
{
    /// <summary>
    /// 关键词
    /// </summary>
    [SugarTable("Cms_Keyword")]
    public class Keyword : BaseTenantEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}