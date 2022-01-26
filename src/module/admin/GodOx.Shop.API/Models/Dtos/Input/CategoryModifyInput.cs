using GodOx.Sys.API.Models.Dtos.Common;
using System;

namespace GodOx.Shop.API.Models.Dtos.Input
{
    public class CategoryModifyInput : GlobalTenantInput
    {
        public int Id { get; set; }
        public int ParentId { get; set; }

        public string IconSrc { get; set; }
        public string Name { get; set; }
        public DateTime ModifyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// Desc:分类集合
        /// Default:-
        /// Nullable:False
        /// </summary>
        public string ParentList { get; set; }

        /// <summary>
        /// Desc分类等级
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Layer { get; set; }
    }
}
