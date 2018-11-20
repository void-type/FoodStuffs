using FoodStuffs.Model.Data;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;
using VoidCore.Model.ClientApp;
using VoidCore.Model.Data;
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
            var applicationSettings = services.AddSettingsSingleton<IApplicationSettings, ApplicationSettings>(_config, true);
            var connectionStrings = services.AddSettingsSingleton<ConnectionStringsSettings>(_config);

            // Infrastructure and authorization
            services.AddSecureTransport();
            services.AddAntiforgery();
            services.AddApiExceptionFilter(_env);

            // Dependencies
            services.AddSqlServerDbContext<FoodStuffsContext>(connectionStrings.FoodStuffs);
            services.AddHttpContextAccessor();
            services.AddSingleton<HttpResponder>();
            services.AddSingleton<IUserNameFormatStrategy, AdLoginUserNameFormatStrategy>();
            services.AddSingleton<ICurrentUserAccessor, WebCurrentUserAccessor>();
            services.AddSingleton<ILoggingStrategy, HttpRequestLoggingStrategy>();
            services.AddSingleton<ILoggingService, MicrosoftLoggerAdapter>();
            services.AddSingleton<IDateTimeService, UtcNowDateTimeService>();
            services.AddSingleton<IAuditUpdater, AuditUpdater>();
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();

            // Domain Events
            services.AddDomainEvents();
        }

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
    }
}
