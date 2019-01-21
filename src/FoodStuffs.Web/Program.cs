using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using VoidCore.AspNet.Logging;

namespace FoodStuffs.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

            var loggingSettings = host.Services.GetRequiredService<LoggingSettings>();
            Log.Logger = SerilogFileLoggerFactory.Create<Startup>(loggingSettings);

            try
            {
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
