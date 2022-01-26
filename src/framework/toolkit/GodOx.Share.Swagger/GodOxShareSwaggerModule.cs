using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.ModuleCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GodOx.Share.Swagger
{
    /// <summary>
    /// 现在没有使用  ，使用的是扩展方法。也可以当成组件来使用
    /// </summary>
    public class GodOxShareSwaggerModule : AppModule
    {
        bool jwtEnable = false;
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"接口文档——{RuntimeInformation.FrameworkDescription}", Version = "v1", Description = "HTTP API" });
                c.OrderActionsBy(o => o.RelativePath);
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var files = Directory.GetFiles(basePath, "*.xml");
                foreach (var file in files)
                {
                    c.IncludeXmlComments(file, true);
                }

                c.CustomOperationIds(apiDesc =>
                 {
                     return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
                 });

                // TODO:一定要返回true！
                c.DocInclusionPredicate((docName, description) =>
                {
                    return true;
                });

                //https://github.com/domaindrivendev/Swashbuckle.AspNetCore  
                jwtEnable = !string.IsNullOrEmpty(context.Configuration["JwtConfig:IsEnable"]) ? Convert.ToBoolean(context.Configuration["JwtConfig:IsEnable"]) : false;
                if (jwtEnable)
                {
                    // 开启加权小锁
                    c.OperationFilter<AddResponseHeadersFilter>();
                    c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                    //// 在header中添加token，传递到后台
                    c.OperationFilter<SecurityRequirementsOperationFilter>();  // 很重要！这里配置安全校验，和之前的版本不一样
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = "Authorization",//jwt默认的参数名称
                        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                    });
                    // c.AddFluentValidationRules();
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                       new OpenApiSecurityScheme{
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                       },
                       new[] { "readAccess", "writeAccess" }
                    }
                });
                }

            });
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Example);
                c.DefaultModelsExpandDepth(-1);
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.MaxDisplayedTags(int.MaxValue);
                c.ShowExtensions();
                c.EnableValidator();
                if (jwtEnable)
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GodOx Admin API v1");
                }
                else
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GodOx MnApp API v1");
                }
                c.RoutePrefix = string.Empty;
                //swagger 中配置MiniProfiler只能使用实例方法
                //c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("GodOx.API.Hosting.index.html");
            });
        }
    }
}
