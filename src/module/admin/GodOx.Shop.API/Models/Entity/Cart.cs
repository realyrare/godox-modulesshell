using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

namespace GodOx.Shop.API.Models.Entity
{
    [SugarTable("shop_cart")]
    public class Cart : BaseTenantEntity
    {
        public int GoodsNum { get; set; }
        public int AppUserId { get; set; }
        public int GoodsId { get; set; }
        public string SpecSkuId { get; set; }
    }
}
