
using GodOx.Auth.API.Models.Common;
using SqlSugar;

/*************************************
* 类名：Shop_Category
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/9 16:51:33
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Models.Entity
{
    [SugarTable("shop_category")]
    public class Category : BaseTenantTreeEntity
    {
        public string IconSrc { get; set; }
        public string Name { get; set; }
    }
}