using SqlSugar;
using System;

/*************************************
* 类名：Common
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/3/30 17:24:54
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Auth.API.Models.Common
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime? ModifyTime { get; set; }
        DateTime CreateTime { get; set; }
        /// <summary>
        /// true为正常，  false为删除
        /// </summary>
        bool Status { get; set; }
    }


    /// <summary>
    /// 所有多租户数据库实体基类
    /// </summary>
    public class BaseTenantEntity : IGlobalTenant, IEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string TenantName { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Status { get; set; } = true;
    }

    public class BaseTenantTreeEntity : BaseTenantEntity
    {
        // Desc:栏位集合    
        public string ParentList { get; set; }
        /// Desc:栏位等级     
        public int Layer { get; set; }
        public int ParentId { get; set; }
    }
}