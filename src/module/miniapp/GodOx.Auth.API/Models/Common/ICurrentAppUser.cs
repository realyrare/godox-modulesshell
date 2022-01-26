namespace GodOx.Auth.API.Models.Common
{
    public interface ICurrentAppUser
    {
        public int AppUserId { get; set; }
        public int TenantId { get; set; }
    }
}
