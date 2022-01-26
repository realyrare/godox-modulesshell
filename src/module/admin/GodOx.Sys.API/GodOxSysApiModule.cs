using AutoMapper;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.ModuleCore.Extensions;
using GodOx.Share.Caches;
using GodOx.Share.FileManage;
using GodOx.Share.Repository;
using GodOx.Sys.API.Common;
using GodOx.Sys.API.Jwt;
using GodOx.Sys.API.Jwt.Extension;
using GodOx.Sys.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GodOx.Sys.API
{
    [DependsOn
        (
        typeof(GodOxShareFileManageModule),
        typeof(GodOxShareRepositoryModule),
        typeof(GodOxShareCachesModule)
     )]
    public class GodOxSysApiModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapper(typeof(AutomapperProfile));
            var jwtEnable = !string.IsNullOrEmpty(context.Configuration["JwtConfig:IsEnable"]) ? Convert.ToBoolean(context.Configuration["JwtConfig:IsEnable"]) : true;
            if (jwtEnable)
            {
                //context.Services.AddSwaggerSetup();
                //注入MiniProfiler
                // context.Services.AddMiniProfiler(options =>
                //     options.RouteBasePath = "/profiler"
                //);
                context.Services.AddAuthorizationSetup(context.Configuration);
            }
            context.Services.AddScoped<IMenuService, MenuService>();
            context.Services.AddScoped<IRecycleService, RecycleService>();
            context.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
            WebHelper.allDbTable = context.Configuration["DbTable:Value"];
            context.Services.AddScoped<JwtHelper>();
            // context.Services.AddHostedService<TimedBackgroundService>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            //加入健康检查中间件
            // app.UseHealthChecks("/health");
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = context.Configuration["ConnectionStrings:MySql"];
        }
    }
}
