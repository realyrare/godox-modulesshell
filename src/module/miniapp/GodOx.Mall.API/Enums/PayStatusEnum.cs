/*************************************
* 类名：PayStatusEnum
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/10 20:02:05
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

using System.ComponentModel;

namespace GodOx.Mall.API.Enums
{
    public enum PayStatusEnum
    {
        /// <summary>
        /// 待付款
        /// </summary>
        [Description("待付款")]
        WaitForPay = 10,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Paid = 20
    }
}