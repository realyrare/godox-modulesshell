using Microsoft.Extensions.DependencyInjection;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;

/*************************************
* 类名：GodOxShareRabbitMqModule
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/9/30 14:25:49
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Share.MsgQueue
{
    public class GodOxShareMsgQueueModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<RabbitMQHelper>();
        }
    }
}