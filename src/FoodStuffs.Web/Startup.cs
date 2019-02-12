using FoodStuffs.Model.Data;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data;
using FoodStuffs.Web.Data.EntityFramework;
using FoodStuffs.Web.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Data;
using VoidCore.AspNet.Logging;
using VoidCore.AspNet.Routing;
using VoidCore.AspNet.Security;
using VoidCore.AspNet.Settings;
using VoidCore.Model.Data;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;
using VoidCore.Model.Users;

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
            // TODO: when updating versions of .Net Core, update this to get the newest behavior.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Settings
            services.AddSettingsSingleton<ApplicationSettings>(_config, true);
            var connectionStrings = services.AddSettingsSingleton<ConnectionStringsSettings>(_config);
            services.AddSettingsSingleton<LoggingSettings>(_config);

            // Infrastructure and authorization
            services.AddSecureTransport(_env);
            services.AddApiExceptionFilter(_env);
            services.AddAntiforgery(_env);

            // Dependencies
            services.AddSqlServerDbContext<FoodStuffsContext>(connectionStrings.FoodStuffs);
            services.AddHttpContextAccessor();
            services.AddSingleton<HttpResponder>();
            services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();
            services.AddSingleton<ILoggingStrategy, HttpRequestLoggingStrategy>();
            services.AddSingleton<ILoggingService, MicrosoftLoggerAdapter>();
            services.AddSingleton<IDateTimeService, NowDateTimeService>();
            services.AddSingleton<IAuditUpdater, AuditUpdater>();
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();

            // Domain Events
            services.AddDomainEvents();
        }

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
    }
}
