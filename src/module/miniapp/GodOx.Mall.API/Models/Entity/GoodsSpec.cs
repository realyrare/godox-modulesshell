
using GodOx.Auth.API.Models.Common;
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

namespace GodOx.Mall.API.Models.Entity
{
    [SugarTable("shop_Goods_Spec")]
    public class GoodsSpec : BaseTenantEntity
    {
        public int GoodsId { get; set; }
        public string GoodsNo { get; set; }
        public decimal GoodsPrice { get; set; }
        /// <summary>
        /// 商品划线价
        /// </summary>
        public decimal LinePrice { get; set; }
        public int StockNum { get; set; }
        /// <summary>
        /// 商品销量
        /// </summary>
        public int GoodsSales { get; set; }
        public double GoodsWeight { get; set; }
        /// <summary>
        /// 商品spu标识
        /// </summary>
        public string SpecSkuId { get; set; }

    }
}