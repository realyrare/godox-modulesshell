namespace GodOx.Sys.API.Models.Dtos.Input
{
    public class RoleMenuBtnInput
    {
        public int RoleId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuId { get; set; }
        public string BtnCodeId { get; set; }
        public bool Status { get; set; }
    }
}
