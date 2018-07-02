using Core.Services.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace FoodStuffs.Web
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

        public static int Main(string[] args)
        {
            var assemblyName = typeof(Program).Assembly.GetName().Name;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Log.Logger = SerilogFileLoggerFactory.Create(assemblyName, environmentName);

            try
            {
                var host = BuildWebHost(args);
                Log.Information("Starting web host.");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
