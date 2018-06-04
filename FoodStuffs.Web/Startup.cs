using Core.Model.Logging;
using Core.Model.Time;
using Core.Services.Action;
using Core.Services.Configuration;
using Core.Services.Logging;
using Core.Services.Time;
using FoodStuffs.Model.Data;
using FoodStuffs.Services.Configuration;
using FoodStuffs.Services.Data;
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
            if (env.IsDevelopment())
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
                    name: "spa-fallback",
                    template: "{*url}",
                    defaults : new { controller = "Home", action = "Index" });
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var applicationSettings = services.ConfigureSettings<ApplicationSettings>(_configuration);
            services.AddSingleton(applicationSettings);

            services.AddMvcAntiforgery();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
            services.AddTransient<IDateTimeService, UtcNowDateTimeService>();
            services.AddTransient<HttpActionResultResponder>();

            services.AddFoodStuffsDbContext(applicationSettings);
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();
        }

        private readonly IConfiguration _configuration;
    }
}