using System;

namespace GodOx.Shop.API.Models.Dtos.Input
{
    public class GoodsModifyInput : GoodsInput
    {
        public int Id { get; set; }

        public DateTime ModifyTime { get; set; } = DateTime.Now;

    }
}
