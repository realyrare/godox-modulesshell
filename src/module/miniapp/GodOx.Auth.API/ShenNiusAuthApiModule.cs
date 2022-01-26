using GodOx.Auth.API.Common;
using GodOx.Auth.API.Configs;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.Share.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace GodOx.Auth.API
{
    [DependsOn(typeof(GodOxShareRepositoryModule))]
    public class GodOxAuthApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            AppConfig.AppId = context.Configuration[""];
            AppConfig.AppSecret = context.Configuration[""];

            context.Services.AddScoped<HttpHelper>();
            context.Services.AddHttpClient();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
