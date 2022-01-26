using GodOx.Blog.API;
using GodOx.Mall.API;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.ModuleCore.Extensions;
using GodOx.Share.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace GodOx.MnApp.Hosting
{
    [DependsOn(
        typeof(GodOxMallAPIModule),
        typeof(GodOxShareSwaggerModule),
        typeof(GodOxBlogApiModule)
        )]
    public class GodOxMnAppHostModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 跨域配置
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            var mvcBuilder = context.Services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(GlobalExceptionFilter));
                //全局多租户
                //options.Filters.Add(typeof(MultiTenantAttribute));
            });

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });


        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 使用HSTS的中间件，该中间件添加了严格传输安全头
            app.UseHsts();
            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            // 跨域
            app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // 异常处理中间件
            //app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // 路由映射           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
