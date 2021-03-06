using GodOx.Sys.API.Enums;
using GodOx.Sys.API.Enums.Extension;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NLog;
using GodOx.Sys.API.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GodOx.Sys.API.Common
{
    /// <summary>
    /// 审计日志
    /// </summary>
    public class LogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }
        private string ActionArguments { get; set; }
        private Stopwatch Stopwatch { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ActionArguments = JsonConvert.SerializeObject(context.ActionArguments);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            base.OnActionExecuting(context);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            Stopwatch.Stop();

            var url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
            var method = context.HttpContext.Request.Method;

            var qs = ActionArguments;
            var userName = context.HttpContext.User.Identity.Name;

            var str = $"地址：{url} \n " +
                $"方式：{method} \n " +
                $"参数：{qs}\n " +
                //$"结果：{res}\n " +
                $"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒";
            if (string.IsNullOrEmpty(LogType))
            {
                Dictionary<string, string> dic = new Dictionary<string, string>
                 {
                { "POST", LogEnum.Add.GetEnumText() },
                { "PUT", LogEnum.Update.GetEnumText() },
                { "DELETE", LogEnum.Delete.GetEnumText() },
                { "GET", LogEnum.Read.GetEnumText() },
                };
                foreach (var item in dic)
                {
                    if (method.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        LogType = item.Value;
                    }
                    else
                    {
                        LogType = method;
                    }
                }
            }
            new LogHelper().Process(userName, LogType, str, LogLevel.Trace);
        }
    }
}
