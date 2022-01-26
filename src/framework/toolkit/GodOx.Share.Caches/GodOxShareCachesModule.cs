using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using System;

/*************************************
* 类名：GodOxShareCachesModule
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/9/29 14:11:51
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Share.Caches
{
    public class GodOxShareCachesModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //redis和cache配置
            var redisEnable = Convert.ToBoolean(context.Configuration["Redis:Enable"]);
            var connection = context.Configuration["Redis:Connection"];
            var instanceName = context.Configuration["Redis:InstanceName"];
            var database = Convert.ToInt32(context.Configuration["Redis:Database"]);

            if (!string.IsNullOrEmpty(connection) && redisEnable && !string.IsNullOrEmpty(instanceName))
            {
                var options = new RedisCacheOptions
                {
                    InstanceName = instanceName,
                    Configuration = connection
                };
                var redis = new RedisCacheHelper(options, database);
                context.Services.AddSingleton(redis);
                context.Services.AddSingleton<ICacheHelper>(redis);
            }
            else
            {
                context.Services.AddMemoryCache();
                context.Services.AddSingleton<ICacheHelper, MemoryCacheHelper>();
            }
        }
    }
}