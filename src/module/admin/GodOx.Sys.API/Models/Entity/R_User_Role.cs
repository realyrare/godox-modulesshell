using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

namespace GodOx.Sys.API.Models.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Sys_R_User_Role")]
    public partial class R_User_Role : BaseEntity
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int UserId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int RoleId { get; set; }

    }
}
