namespace GodOx.Sys.API.Models.Entity.Common
{
    /// <summary>
    /// 全局多租户id
    /// </summary>
    public interface IGlobalTenant
    {
        public int TenantId { get; set; }
    }
}
