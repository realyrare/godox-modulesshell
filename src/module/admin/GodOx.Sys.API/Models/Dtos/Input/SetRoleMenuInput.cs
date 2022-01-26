using System.Collections.Generic;

namespace GodOx.Sys.API.Models.Dtos.Input
{
    public class SetRoleMenuInput
    {
        public int RoleId { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
