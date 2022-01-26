using System.ComponentModel;

/*************************************
* 类名：SpecTypeEnum
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/10 19:38:04
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mall.API.Enums
{
    public enum DeductStockTypeEnum
    {
        /// <summary>
        /// 下单减库存
        /// </summary>
        [Description("下单减库存")]
        PlaceOrder = 10,
        /// <summary>
        ///付款减库存
        /// </summary>
        [Description("付款减库存")]
        Pay = 20
    }
}