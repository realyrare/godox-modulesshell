using System;

/*************************************
* 类名：AppUserAddressOutput
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/23 11:52:02
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Shop.API.Models.Dtos.Output
{
    public class AppUserAddressOutput
    {
        /// <summary>
        /// Desc:收货人姓名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }
        public string AppUserName { get; set; }
        public string TenantName { get; set; }

        /// <summary>
        /// Desc:联系电话
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Phone { get; set; }

        /// <summary>
        /// Desc:所在省份id
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public string Province { get; set; }

        /// <summary>
        /// Desc:所在城市id
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public string City { get; set; }

        /// <summary>
        /// Desc:所在区id
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public string Region { get; set; }

        /// <summary>
        /// Desc:详细地址
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Detail { get; set; }

        public DateTime CreateTime { get; set; }

    }
}