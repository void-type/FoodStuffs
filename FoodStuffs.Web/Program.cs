using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using VoidCore.AspNet.Logging;

namespace FoodStuffs.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // TODO: MS warnings are suppressed in logs.
            Log.Logger = SerilogFileLoggerFactory.Create<Startup>(true);

            try
            {
                var host = WebHost
                    .CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build();

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
