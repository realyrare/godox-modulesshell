using System;

namespace GodOx.Sys.API.Models.Dtos.Input
{
    public class RoleInput
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
