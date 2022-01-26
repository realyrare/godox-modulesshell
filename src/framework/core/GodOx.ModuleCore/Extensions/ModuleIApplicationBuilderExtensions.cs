using GodOx.ModuleCore.Manage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GodOx.ModuleCore.Extensions
{
    public static class ModuleIApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用Module
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static void UseModule(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            serviceProvider.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;

            var moduleManager = serviceProvider.GetService<IModuleManager>();
            moduleManager.ApplicationInitialization(serviceProvider);
        }
    }
}
