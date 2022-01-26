using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GodOx.Share.Repository
{
    public class GodOxShareRepositoryModule : AppModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //注册服务
            string connectionStr = context.Configuration["ConnectionStrings:MySql"];
            if (string.IsNullOrEmpty(connectionStr))
            {
                throw new ArgumentException("data connectionStr is not fuond");
            }
            DbContext._connectionStr = connectionStr;

            //注入泛型BaseServer
            context.Services.AddScoped(typeof(IBaseServer<>), typeof(BaseServer<>));
            context.Services.AddScoped<DbContext>();
        }
    }
}
