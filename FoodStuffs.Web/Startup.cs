using Core.Model.Services.Logging;
using Core.Model.Services.Time;
using Core.Services.Action;
using Core.Services.Logging;
using Core.Services.Time;
using FoodStuffs.Services.EntityFramework;
using FoodStuffs.Services.Data;
using FoodStuffs.Model.Data;
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => 
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            services.AddAntiforgery(options =>
                options.HeaderName = "X-CSRF-TOKEN");
            services.AddSingleton(_configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
            services.AddTransient<IDateTimeService, UtcNowDateTimeService>();
            services.AddTransient<HttpActionResultResponder>();
            services.AddDbContext<FoodStuffsContext>(options =>
                options.UseSqlServer(_configuration["FoodStuffsConnectionString"]));
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();
        }

        private readonly IConfiguration _configuration;
    }
}