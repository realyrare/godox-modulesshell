using AutoMapper;
using GodOx.Auth.API;
using GodOx.Mall.API.Services;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using Microsoft.Extensions.DependencyInjection;

namespace GodOx.Mall.API
{
    [DependsOn
       (typeof(GodOxAuthApiModule))]
    public class GodOxMallAPIModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<IGoodsService, GoodsService>();
            context.Services.AddScoped<IOrderGoodsService, OrderGoodsService>();
            context.Services.AddScoped<IOrderService, OrderService>();
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
