using GodOx.Auth.API.Models.Common;
using GodOx.Auth.API.Models.Dtos.Output;
using GodOx.Share.Caches;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using GodOx.Auth.API;
using GodOx.Auth.API.Configs;
using System.Linq;

namespace GodOx.Auth.API.Attributes
{
    /// <summary>
    /// 审计日志
    /// </summary>
    public class AppAuthAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["token"].FirstOrDefault() ?? context.HttpContext.Request.Query["token"].FirstOrDefault() ?? context.HttpContext.Request.Form["token"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                ReturnResult(context, "很抱歉,您未登录！", StatusCodes.Status401Unauthorized);
                return;
            }
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService(typeof(ICacheHelper)) as ICacheHelper;
            var httpWx = cache.Get<HttpWxUserOutput>(token);
            if (httpWx == null)
            {
                ReturnResult(context, "未登录!缺少必要的参数：token失效了", StatusCodes.Status401Unauthorized);
                return;
            }
            new CurrentAppContext(httpWx);
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            foreach (var parameter in actionDescriptor.Parameters)
            {
                var parameterName = parameter.Name;//获取Action方法中参数的名字
                var parameterType = parameter.ParameterType;//获取Action方法中参数的类型
                                                            //if (!typeof(int).IsAssignableFrom(parameterType))//如果不是ID类型
                                                            //{
                                                            //    continue;
                                                            //}
                                                            //自动添加租户id
                if (typeof(IGlobalTenant).IsAssignableFrom(parameterType))
                {
                    var model = context.ActionArguments[parameterName] as ICurrentAppUser;
                    if (httpWx != null)
                    {
                        model.TenantId = httpWx.TenantId;
                        model.AppUserId = httpWx.AppUserId;
                    }
                }
            }


            base.OnActionExecuting(context);
        }

        private static void ReturnResult(ActionExecutingContext context, string msg, int statusCodes)
        {
            context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            context.Result = new JsonResult(new ApiResult(msg, statusCodes), setting);
        }

    }


}
