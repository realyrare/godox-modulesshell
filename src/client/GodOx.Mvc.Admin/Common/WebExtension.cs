using System;

/*************************************
* 类名：StaticExtension
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/2 18:06:12
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Mvc.Admin.Common
{
    public static class WebExtension
    {
        public static string ToWebString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string TrimEnd(this string sourceStr, string removeStr)
        {
            if (string.IsNullOrEmpty(sourceStr) || !sourceStr.EndsWith(removeStr))
            {
                return sourceStr;
            }
            return sourceStr.Substring(0, sourceStr.Length - removeStr.Length);
        }
    }
}