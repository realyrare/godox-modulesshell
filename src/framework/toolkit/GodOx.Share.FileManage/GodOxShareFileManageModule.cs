using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GodOx.Share.FileManage
{
    public class GodOxShareFileManageModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //是否启用本地文件上传 选择性注入
            var enableLocalFile = Convert.ToBoolean(context.Configuration["LocalFile:IsEnable"]);
            if (enableLocalFile)
            {
                context.Services.AddSingleton<IUploadFile, LocalFile>();
            }
            else
            {
                //七牛云配置信息读取
                context.Services.Configure<QiNiuOss>(context.Configuration.GetSection("QiNiuOss"));
                context.Services.AddSingleton<IUploadFile, QiniuCloud>();
            }
        }
    }
}
