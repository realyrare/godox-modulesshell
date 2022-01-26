using GodOx.ModuleCore.Manage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GodOx.ModuleCore.Extensions
{
    public static class ModuleServiceCollectionExtensions
    {
        /// <summary>
        /// 添加模块服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddModule<T>(this IServiceCollection services, IConfiguration configuration)
            where T : IAppModule
        {
            ModuleManager moduleManager = new ModuleManager();
            moduleManager.StartModule<T>(services);
            moduleManager.ConfigurationService(services, configuration);

            services.TryAddSingleton<IModuleManager>(moduleManager);
            var obj = new ObjectAccessor<IApplicationBuilder>();
            services.AddObjectAccessor(obj);
            return services;
        }
    }
}
