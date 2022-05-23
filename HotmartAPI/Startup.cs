using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace HotmartAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var herukuEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_HEROKU_ENVIRONMENT");
            if (herukuEnvironment is ("Development" or "Production"))
            {
                HotmartConfig = new HotmartConfiguration
                {
                    ApiToken = Environment.GetEnvironmentVariable("ApiToken"),
                    Hottok = Environment.GetEnvironmentVariable("Hottok"),
                };
                return;
            }

            HotmartConfig = configuration.GetSection("HotmartConfiguration").Get<HotmartConfiguration>();
        }

        public IConfiguration Configuration { get; }
        public static HotmartConfiguration HotmartConfig { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "hotmartWebhook",
                                             pattern: $"api/webhook/hotmart",
                                             new { controller = "Hotmart", action = "Post" });
                endpoints.MapControllers();
            });
        }
    }
}
