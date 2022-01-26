using AutoMapper;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.Sys.API;

namespace GodOx.Cms.API
{
    [DependsOn(
       typeof(GodOxSysApiModule)
      )]
    public class GodOxCmsApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
