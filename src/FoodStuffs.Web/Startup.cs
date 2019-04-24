using FoodStuffs.Model.Data;
using FoodStuffs.Web.Auth;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Data;
using VoidCore.AspNet.Logging;
using VoidCore.AspNet.Routing;
using VoidCore.AspNet.Security;
using VoidCore.AspNet.Settings;
using VoidCore.Model.Auth;
using VoidCore.Model.Logging;
using VoidCore.Model.Time;

namespace FoodStuffs.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSpaExceptionPage(_env)
                .UseSecureTransport(_env)
                .UseSecurityHeaders(_env)
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
            var loggingSettings = services.AddSettingsSingleton<LoggingSettings>(_config);

            // Infrastructure
            Log.Logger = SerilogFileLoggerFactory.Create<Startup>(loggingSettings);
            services.AddSecureTransport(_env);
            services.AddApiExceptionFilter();
            services.AddApiAntiforgery();
            services.AddSqlServerDbContext<FoodStuffsContext>(_env, connectionStrings["FoodStuffs"]);
            services.AddHttpContextAccessor();

            // Model Dependencies
            services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();
            services.AddSingleton<ILoggingStrategy, HttpRequestLoggingStrategy>();
            services.AddSingleton<ILoggingService, MicrosoftLoggerAdapter>();
            services.AddSingleton<IDateTimeService, NowDateTimeService>();
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();

            // Domain Events
            services.AddDomainEvents();
        }
    }
}
