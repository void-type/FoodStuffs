using FoodStuffs.Model.Data;
using FoodStuffs.Web.Auth;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using VoidCore.AspNet.ClientApp;
using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;
using VoidCore.AspNet.Routing;
using VoidCore.AspNet.Security;
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
                .UseRequestLoggingScope()
                .UseSerilogRequestLogging()
                .UseCurrentUserLogging()
                .UseSpaEndpoints();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Settings
            services.AddSettingsSingleton<WebApplicationSettings>(_config, true).Validate();

            // Infrastructure
            services.AddControllers();
            services.AddSpaSecurityServices(_env);
            services.AddApiExceptionFilter();

            // Authorization

            // Dependencies
            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();
            services.AddSingleton<IDateTimeService, NowDateTimeService>();

            _config.GetRequiredConnectionString<FoodStuffsContext>();
            services.AddDbContext<FoodStuffsContext>();
            services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();

            // Auto-register Domain Events
            services.AddDomainEvents(
                ServiceLifetime.Scoped,
                typeof(GetWebClientInfo).Assembly,
                typeof(IFoodStuffsData).Assembly);
        }
    }
}
