using FoodStuffs.Model.Data;
using FoodStuffs.Web.Auth;
using FoodStuffs.Web.Configuration;
using FoodStuffs.Web.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
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
    public static class Program
    {
        public static int Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseSerilog();

            var env = builder.Environment;
            var config = builder.Configuration;
            var services = builder.Services;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Configuring host for {Name} v{Version}", ThisAssembly.AssemblyTitle, ThisAssembly.AssemblyInformationalVersion);

                // Settings
                services.AddSettingsSingleton<WebApplicationSettings>(config, true).Validate();

                // Infrastructure
                services.AddControllers();
                services.AddSpaSecurityServices(env);
                services.AddApiExceptionFilter();

                // Authorization

                // Dependencies
                services.AddHttpContextAccessor();
                services.AddSingleton<ICurrentUserAccessor, SingleUserAccessor>();
                services.AddSingleton<IDateTimeService, NowDateTimeService>();

                config.GetRequiredConnectionString<FoodStuffsContext>();
                services.AddDbContext<FoodStuffsContext>();
                services.AddScoped<IFoodStuffsData, FoodStuffsEfData>();

                // Auto-register Domain Events
                services.AddDomainEvents(
                    ServiceLifetime.Scoped,
                    typeof(GetWebClientInfo).Assembly,
                    typeof(IFoodStuffsData).Assembly);

                services.AddSwaggerWithCsp(env);

                var app = builder.Build();

                app.UseSpaExceptionPage(env)
                    .UseSecureTransport(env)
                    .UseSecurityHeaders(env)
                    .UseStaticFiles()
                    .UseRouting()
                    .UseRequestLoggingScope()
                    .UseSerilogRequestLogging()
                    .UseCurrentUserLogging()
                    .UseSwaggerAndUi(env)
                    .UseSpaEndpoints();

                Log.Information("Starting host.");
                app.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.Information("Stopping host.");
                Log.CloseAndFlush();
            }
        }
    }
}
