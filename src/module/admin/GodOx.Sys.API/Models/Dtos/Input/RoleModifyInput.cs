using System;

namespace GodOx.Sys.API.Models.Dtos.Input
{
    public class RoleModifyInput
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
