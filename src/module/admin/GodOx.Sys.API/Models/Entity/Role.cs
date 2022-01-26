using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

namespace GodOx.Sys.API.Models.Entity
{
    ///<summary>
    /// 权限角色表
    ///</summary>
    [SugarTable("Sys_Role")]
    public partial class Role : BaseEntity
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
