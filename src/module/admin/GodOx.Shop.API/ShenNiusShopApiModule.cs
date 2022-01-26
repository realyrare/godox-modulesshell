using AutoMapper;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.Shop.API.Services;
using GodOx.Sys.API;
using Microsoft.Extensions.DependencyInjection;

namespace GodOx.Shop.API
{
    [DependsOn(
    typeof(GodOxSysApiModule)
   )]
    public class GodOxShopApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            context.Services.AddScoped<IGoodsService, GoodsService>();
            context.Services.AddScoped<IOrderService, OrderService>();
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
