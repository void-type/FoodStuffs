using FoodStuffs.Model.Data;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.Action;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;
using VoidCore.AspNet.Time;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;

namespace FoodStuffs.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionPage(_environment)
                .UseStaticFiles()
                .AddMvcSpaRoute();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSettingsSingleton<ApplicationSettings>(_configuration);
            var connectionStrings = services
                .AddSettingsSingleton<ConnectionStringSettings>(_configuration.GetSection(ConnectionStringSettings.SectionName));

            services.AddMvcAntiforgery();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ILoggingService, ActionToAspNetLoggerAdapter>();
            services.AddTransient<IDateTimeService, UtcNowDateTimeService>();
            services.AddTransient<HttpActionResultResponder>();

            services.AddSqlServerDbContext<FoodStuffsContext>(connectionStrings.FoodStuffs);
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
    }
}
