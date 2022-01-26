

using GodOx.Auth.API.Models.Common;
using SqlSugar;

namespace GodOx.Mall.API.Models.Entity
{
    [SugarTable("shop_cart")]
    public partial class Cart : BaseTenantEntity
    {
        public int GoodsNum { get; set; }
        public int AppUserId { get; set; }
        public int GoodsId { get; set; }
        public string SpecSkuId { get; set; }
    }
}
