/*************************************
* 类名：GlobalTenantInput
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/30 18:15:44
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

using GodOx.Sys.API.Models.Entity.Common;

namespace GodOx.Sys.API.Models.Dtos.Common
{
    /// <summary>
    /// 多租户约定方便Add ,Modify使用
    /// </summary>
    public class GlobalTenantInput : IGlobalTenant
    {
        public int TenantId { get; set; }
    }
    public class UploadInput
    {
        public string Directory { get; set; }
    }
}