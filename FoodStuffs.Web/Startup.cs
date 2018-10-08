using FoodStuffs.Model.Data;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;
using VoidCore.Model.ClientApp;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;

namespace FoodStuffs.Web
{
    public class Startup
    {
        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSpaExceptionPage(_env)
                .UseSecureTransport(_env)
                .UseStaticFiles()
                .UseSpaMvcRoute();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Get the newest MVC behavior.
            // TODO: for aspnet versions newer than 2.1, update or remove this.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Settings
            services.AddSettingsSingleton<ApplicationSettings>(_config);
            var connectionStrings = services
                .AddSettingsSingleton<ConnectionStringSettings>(_config.GetSection(ConnectionStringSettings.SectionName));

            // Infrastructure and authorization
            services.AddAntiforgery();
            services.AddApiExceptionFilter(_env);

            // Dependencies
            services.AddSqlServerDbContext<FoodStuffsContext>(connectionStrings.FoodStuffs);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpResponder>();
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();
            services.AddScoped<ILoggingService, MicrosoftLoggingAdapter>();
            services.AddScoped<IDateTimeService, UtcNowDateTimeService>();
            services.AddScoped<GetApplicationInfo.IApplicationInfo, WebApplicationInfo>();

            // Domain Events
            services.AddDomainEvents();
        }

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
    }
}
