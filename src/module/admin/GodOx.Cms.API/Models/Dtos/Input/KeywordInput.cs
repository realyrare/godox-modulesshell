using GodOx.Sys.API.Models.Dtos.Common;
using System;

/*************************************
* 类名：KeywordInput
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/31 19:22:32
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Models.Dtos.Input
{
    public class KeywordInput : GlobalTenantInput
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}