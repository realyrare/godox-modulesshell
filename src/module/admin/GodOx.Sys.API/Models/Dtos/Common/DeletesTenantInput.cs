using GodOx.Sys.API.Models.Entity.Common;
using System.Collections.Generic;

namespace GodOx.Sys.API.Models.Dtos.Common
{
    public class DeletesInput
    {
        public List<int> Ids { get; set; } = new List<int>();
    }
    public class DeletesTenantInput : DeletesInput, IGlobalTenant
    {

        public int TenantId { get; set; }
    }
}
