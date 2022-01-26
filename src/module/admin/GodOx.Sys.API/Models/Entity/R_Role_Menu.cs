using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

namespace GodOx.Sys.API.Models.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Sys_R_Role_Menu")]
    public partial class R_Role_Menu : BaseEntity
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int RoleId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public bool IsPass { get; set; } = true;
        [SugarColumn(IsJson = true)]
        public string[] BtnCodeIds { get; set; }

    }
}
