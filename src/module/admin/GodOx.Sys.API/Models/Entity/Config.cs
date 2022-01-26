using GodOx.Sys.API.Models.Entity.Common;
using SqlSugar;

namespace GodOx.Sys.API.Models.Entity
{
    ///<summary>
    /// 字典表
    ///</summary>
    [SugarTable("Sys_Config")]
    public partial class Config : BaseEntity
    {
        /// <summary>
        /// Desc:字典类型标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int ParentId { get; set; }

        /// <summary>
        /// Desc:字典值——名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:字典值——英文名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string EnName { get; set; }

        /// <summary>
        /// Desc:字典值——描述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Summary { get; set; }
        public string Type { get; set; }

    }
}
