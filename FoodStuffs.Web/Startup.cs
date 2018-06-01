using Core.Model.Services.Logging;
using Core.Model.Services.Time;
using Core.Services.Action;
using Core.Services.Logging;
using Core.Services.Time;
using Core.Services.WebServerConfiguration;
using FoodStuffs.Model.Data;
using FoodStuffs.Services.Data;
using FoodStuffs.Services.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStuffs.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "catch",
                    template: "{*url}",
                    defaults: new {controller = "Home", action = "Index"});
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddMvcAntiforgery();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
            services.AddTransient<IDateTimeService, UtcNowDateTimeService>();
            services.AddTransient<HttpActionResultResponder>();
            services.AddFoodStuffsDbContext(_configuration);
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();
        }

        private readonly IConfiguration _configuration;
    }
}