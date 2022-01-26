using GodOx.Sys.API.Models.Dtos.Common;
using System;

/*************************************
* 类名：KeywordModifyInput
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/31 19:23:47
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Models.Dtos.Input
{
    public class KeywordModifyInput : GlobalTenantInput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}