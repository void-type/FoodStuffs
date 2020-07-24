using FoodStuffs.Model.Data;
using FoodStuffs.Web.Auth;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Data;
using VoidCore.AspNet.Logging;
using VoidCore.AspNet.Routing;
using VoidCore.AspNet.Security;
using VoidCore.Domain.Guards;
using VoidCore.Model.Auth;
using VoidCore.Model.Configuration;
using VoidCore.Model.Time;

namespace FoodStuffs.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;

        public Startup(IConfiguration config, IHostEnvironment env)
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
                .UseRouting()
                .UseSerilogRequestLogging()
                .UseSpaEndpoints();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Settings
            services.AddSettingsSingleton<ApplicationSettings>(_config, true);
            var connectionStrings = services.AddSettingsSingleton<ConnectionStringsSettings>(_config);

            // Infrastructure
            services.AddControllers()
            // TODO: Try the built-in JSON support when it supports constructors.
                .AddNewtonsoftJson();
            services.AddSpaSecurityServices(_env);
            services.AddApiExceptionFilter();

            // Authorization

            // Dependencies
            services.AddHttpContextAccessor();
            services.AddWebLoggingAdapters();
            services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();
            services.AddSingleton<IWebAppVariables, WebAppVariables>();
            services.AddSingleton<IDateTimeService, NowDateTimeService>();

            // TODO: how can we make this a singleton (pool?) and then make domain events singletons.
            var connectionString = connectionStrings["FoodStuffs"];
            connectionString.EnsureNotNullOrEmpty(nameof(connectionString), "Connection string not found in application configuration.");

            services.AddDbContextPool<FoodStuffsContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();

            // Auto-register Domain Events
            services.FindAndRegisterDomainEvents(
                ServiceLifetime.Scoped,
                typeof(GetWebClientInfo).Assembly,
                typeof(IFoodStuffsData).Assembly);
        }
    }
}
