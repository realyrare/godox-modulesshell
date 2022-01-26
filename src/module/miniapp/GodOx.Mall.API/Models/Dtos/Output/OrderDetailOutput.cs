using GodOx.Mall.API.Models.Entity;
using System;
using System.Collections.Generic;

namespace GodOx.Mall.API.Models.Dtos.Output
{
    public class OrderDetailOutput : Order
    {
        public string AppUserName { get; set; }
        public OrderAddress Address { get; set; } = new OrderAddress();
        public List<OrderGoodsDetailOutput> GoodsDetailList { get; set; } = new List<OrderGoodsDetailOutput>();
    }
    public class OrderGoodsDetailOutput
    {
        public string GoodsImg { get; set; }
        public string GoodsName { get; set; }
        public string GoodsAttr { get; set; }
        public int GoodsId { get; set; }
        public decimal GoodsPrice { get; set; }
        public double GoodsWeight { get; set; }
        public decimal TotalNum { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public string GoodsNo { get; set; }
    }
}
