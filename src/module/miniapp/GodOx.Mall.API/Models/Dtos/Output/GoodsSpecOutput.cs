using System;

namespace GodOx.Mall.API.Models.Dtos.Output
{
    public class GoodsSpecOutput
    {
        public int GoodsSpecId { get; set; }
        public int GoodsId { get; set; }
        public string GoodsNo { get; set; }
        public decimal GoodsPrice { get; set; }
        public int GoodsSales { get; set; }
        public double GoodsWeight { get; set; }
        public decimal LinePrice { get; set; }
        public string SpecSkuId { get; set; }
        public int StockNum { get; set; }
        public DateTime CreateTime { get; set; }
        public int TenantId { get; set; }
    }
}
