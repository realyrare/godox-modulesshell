using GodOx.Auth.API.Models.Common;
using System;

namespace GodOx.Mall.API.Models.Dtos.Output
{
    public class GoodsDetailOutput : ICurrentAppUser
    {
        public int Id { get; set; }
        public DateTime ModifyTime { get; set; }
        public int AppUserId { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public int SpecType { get; set; }
        /// <summary>
        /// 库存计算方式
        /// </summary>
        public int DeductStockType { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 初始销量
        /// </summary>
        public int SalesInitial { get; set; }
        /// <summary>
        /// 实际销量
        /// </summary>
        public int SalesActual { get; set; }
        /// <summary>
        /// 配送模板id
        /// </summary>
        public int DeliveryId { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public int GoodsStatus { get; set; }
        public string ImgUrl { get; set; }
        /// <summary>
        /// 商品多规格
        /// </summary>
        public string SpecMany { get; set; }
        public GoodsSpecOutput GoodsSpecOutput { get; set; } = new GoodsSpecOutput();

    }
}
