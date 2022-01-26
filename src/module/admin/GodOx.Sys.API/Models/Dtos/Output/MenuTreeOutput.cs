using System.Collections.Generic;

namespace GodOx.Sys.API.Models.Dtos.Output
{
    public class MenuTreeOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // [JsonProperty(PropertyName = "checked")]
        public bool Checked { get; set; } = false;
        public int ParentId { get; set; }
        public List<MenuTreeOutput> Children { get; set; }
        public bool Spread { get; set; } = true;

    }
}
