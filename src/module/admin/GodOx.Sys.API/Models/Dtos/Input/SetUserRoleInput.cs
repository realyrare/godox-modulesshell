namespace GodOx.Sys.API.Models.Dtos.Input
{
    public class SetUserRoleInput
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; } = true;
    }
}
