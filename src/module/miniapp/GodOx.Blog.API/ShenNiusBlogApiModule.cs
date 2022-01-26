using AutoMapper;
using GodOx.Auth.API;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using Microsoft.Extensions.DependencyInjection;

namespace GodOx.Blog.API
{
    [DependsOn(typeof(GodOxAuthApiModule))]
    public class GodOxBlogApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            context.Services.AddHttpClient();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
