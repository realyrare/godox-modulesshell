/*************************************
* 类名：GoodsOutput
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/30 17:28:55
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Models.Dtos.Output
{
    public class CartGoodsOutput
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int SalesActual { get; set; }
        public int GoodsId { get; set; }
        public string ImgUrl { get; set; }

        public decimal GoodsPrice { get; set; }
        /// <summary>
        /// 商品划线价
        /// </summary>
        public decimal LinePrice { get; set; }

        public int GoodsSales { get; set; }

        /// <summary>
        /// 此商品加入购物车的数量
        /// </summary>
        public int OrderTotalNum { get; set; }

        public int SpecType { get; set; }

        public string SpecSkuId { get; set; }

        /// <summary>
        /// 商品多规格
        /// </summary>
        public string SpecMany { get; set; }
    }
}