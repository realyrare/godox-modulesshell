using GodOx.Sys.API.Models.Entity.Common;

namespace GodOx.Sys.API.Models.Dtos.Common
{
    public class GlobalTenantQuery : IGlobalTenant
    {
        public int TenantId { get; set; }
    }
}
