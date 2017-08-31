using FoodStuffs.Data.FoodStuffsDb;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Model.Interfaces.Services.Logging;
using FoodStuffs.Web.Actions;
using FoodStuffs.Web.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            loggerFactory.AddFile(Configuration.GetSection("Logging"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);

            // Choose a db implementation
            //services.AddTransient<IFoodStuffsData, FoodStuffsSqlData>();
            services.AddTransient<IFoodStuffsData, FoodStuffsMemoryData>();

            services.AddTransient<ILoggingService, LoggerAdapter>();
            services.AddTransient<ActionResultResponder>();
        }
    }
}