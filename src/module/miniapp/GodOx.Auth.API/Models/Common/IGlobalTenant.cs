namespace GodOx.Auth.API.Models.Common
{
    /// <summary>
    /// 全局多租户id
    /// </summary>
    public interface IGlobalTenant
    {
        public int TenantId { get; set; }
    }

}
