using GodOx.Share.Caches;
using GodOx.Sys.API.Common;
using GodOx.Sys.API.Models.Entity.Common;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

/*************************************
* 类名：MultiTenant
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/3/30 17:17:23
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Sys.API.Attributes
{
    public class MultiTenantAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 全局注册过滤器 ，自动为添加 更新方法赋值。也可自行手动打上特性标签
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //s  var actionName = actionDescriptor.ActionName.ToLower();
            ICacheHelper cache = context.HttpContext.RequestServices.GetRequiredService<ICacheHelper>();
            ICurrentUserContext currentUserContext = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserContext>();
            var tenantId = cache.Get<int>($"{SysCacheKey.CurrentTenant}:{currentUserContext.Id}");
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
                    var model = context.ActionArguments[parameterName] as IGlobalTenant;
                    if (model != null)
                    {
                        if (tenantId == 0)
                        {
                            //进程内缓存重启后多租户值会丢失！使用持久化的nosql可解决。如果用用进程内缓存，则退出重新登录赋值。
                            throw new ArgumentNullException("缓存获取不到当前的租户值!请退出重新登录！");
                        }
                        model.TenantId = tenantId;
                    }
                }
            }

        }
    }
}