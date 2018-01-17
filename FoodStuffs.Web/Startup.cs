using Core.Model.Services.DateTime;
using Core.Model.Services.Logging;
using FoodStuffs.Data.Services;
using FoodStuffs.Model.Interfaces.Services.Data;
using FoodStuffs.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
      // Add framework services.
      services.AddMvc();
      services.AddSingleton(_configuration);

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddTransient<IFoodStuffsData, FoodStuffsEfSqlData>();
      services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
      services.AddTransient<IDateTimeService, NowDateTimeService>();
      services.AddTransient<HttpActionResultResponder>();
    }
  }
}
