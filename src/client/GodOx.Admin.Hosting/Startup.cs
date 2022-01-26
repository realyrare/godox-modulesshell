using GodOx.ModuleCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GodOx.Admin.Hosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModule<GodOxApiHostingModule>(Configuration);
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseModule();
        }
    }
}
