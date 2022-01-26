
using GodOx.Auth.API.Models.Common;
using SqlSugar;

/*************************************
* 类名：Spec
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/9 17:56:52
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Models.Entity
{
    [SugarTable("shop_Spec")]
    public class Spec : BaseTenantEntity
    {
        public string Name { get; set; }
    }
}