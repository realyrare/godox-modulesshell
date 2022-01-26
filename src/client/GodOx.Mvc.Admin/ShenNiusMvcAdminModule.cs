using FluentValidation.AspNetCore;
using GodOx.Cms.API;
using GodOx.ModuleCore;
using GodOx.ModuleCore.Context;
using GodOx.ModuleCore.Extensions;
using GodOx.Shop.API;
using GodOx.Sys.API;
using GodOx.Sys.API.Attributes;
using GodOx.Sys.API.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using GodOx.Mvc.Admin.Common;
using GodOx.Sys.API.Configs;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GodOx.Mvc.Admin
{
    [DependsOn(
         typeof(GodOxShopApiModule),
          typeof(GodOxCmsApiModule),
          typeof(GodOxSysApiModule)
          )]
    public class GodOxMvcAdminModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSession();
            // 认证
            context.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = "GodOx.Mvc.Admin";
                o.LoginPath = new PathString("/sys/user/login");
                o.Cookie.HttpOnly = true;
            });
            context.Services.AddSignalR();
            var mvcBuilder = context.Services.AddControllersWithViews(options =>
            {
                // options.Filters.Add(new AuthorizeFilter());
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif

            //把控制器当成服务 进行拦截
            // mvcBuilder.AddControllersAsServices();
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                // options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
                options.LowercaseQueryStrings = true;
            });
            // FluentValidation 统一请求参数验证          
            mvcBuilder.AddFluentValidation(options =>
            {
                var arry = new string[] { "GodOx.Sys.API", "GodOx.Shop.API", "GodOx.Cms.API" };
                foreach (var it in arry)
                {
                    var types = Assembly.Load(it).GetTypes()
                 .Where(e => e.Name.EndsWith("Validator"));
                    foreach (var item in types)
                    {
                        options.RegisterValidatorsFromAssemblyContaining(item);
                    }
                }
                options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            // 模型验证自定义返回格式
            context.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(x => x.Errors
                            .Select(p => p.ErrorMessage))
                        .ToList();
                    // string.Join(",", errors.Select(e => string.Format("{0}", e)).ToList())； 一次性把所有未验证的消息都取出来
                    var result = new ApiResult(
                        msg: errors.FirstOrDefault(),
                        statusCode: 400
                    );
                    return new BadRequestObjectResult(result);
                };
            });

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");
                app.UseHsts();
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码
            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new CustomerFileExtensionContentTypeProvider()
            });
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/error.html");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // 路由映射
            app.UseEndpoints(endpoints =>
            {
                //这个扩展方法全局添加也可以代替Authorize,如果因重写了IAuthorizationFilter就可以不添加。
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //全局路由配置
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=index}/{id?}");
                endpoints.MapHub<UserLoginNotifiHub>("userLoginNotifiHub");
            });
        }
    }
}
