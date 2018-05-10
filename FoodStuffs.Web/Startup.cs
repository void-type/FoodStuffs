using Core.Model.Services.Logging;
using Core.Model.Services.Time;
using FoodStuffs.Data.Models;
using FoodStuffs.Data.Service;
using FoodStuffs.Model.Data;
using FoodStuffs.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsEnvironment("Production"))
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(_configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IFoodStuffsData, FoodStuffsEfData>();
            services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
            services.AddTransient<IDateTimeService, UtcNowDateTimeService>();
            services.AddTransient<HttpActionResultResponder>();

            if (_configuration["FoodStuffsConnectionString"] == "In-Memory")
            {
                services.AddDbContext<FoodStuffsContext>(options =>
                    options.UseInMemoryDatabase("FoodStuffsDev"));
            }
            else
            {
                services.AddDbContext<FoodStuffsContext>(options =>
                    options.UseSqlServer(_configuration["FoodStuffsConnectionString"]));
            }
        }

        private readonly IConfiguration _configuration;
    }
}