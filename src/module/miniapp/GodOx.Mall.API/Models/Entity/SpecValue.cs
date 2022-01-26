
using GodOx.Auth.API.Models.Common;
using SqlSugar;

/*************************************
* 类名：Spec_Value
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/9 18:11:21
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Models.Entity
{
    [SugarTable("shop_Spec_Value")]
    public class SpecValue : BaseTenantEntity
    {
        public string Value { get; set; }
        public int SpecId { get; set; }
    }
}