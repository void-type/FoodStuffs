using Core.Model.Services.DateTime;
using Core.Model.Services.Logging;
using FoodStuffs.Data.Services;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Web.Services.Actions;
using FoodStuffs.Web.Services.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Web
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
                  "default",
                  "{controller=Home}/{action=Index}");
      });
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddMvc();
      services.AddSingleton(_configuration);

      // Choose a db implementation
      services.AddTransient<IFoodStuffsData, FoodStuffsEfSqlData>();
      //services.AddTransient<IFoodStuffsData, FoodStuffsEfMemoryData>();

      // Add other dependencies
      services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
      services.AddTransient<IDateTimeService, NowDateTimeService>();
      services.AddTransient<HttpActionResultResponder>();
    }
  }
}
