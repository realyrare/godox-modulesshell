using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

/*************************************
* 类名：Goods_Spec
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/9 18:02:16
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Shop.API.Models.Entity
{
    /// <summary>
    /// 商品规格，规格值多对多的表
    /// </summary>
    [SugarTable("shop_goods_spec_rel")]
    public class GoodsSpecRel : BaseTenantEntity
    {
        public int GoodsId { get; set; }

        /// <summary>
        /// 商品规格id
        /// </summary>
        public int SpecId { get; set; }
        /// <summary>
        /// 商品规格值id
        /// </summary>
        public int SpecValueId { get; set; }

    }
}