using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Generic;
using System;
using VoidCore.AspNet.Logging;

namespace FoodStuffs.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

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

        public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var configurationDefaults = new Dictionary<string, string>
                    {
                        {"Serilog:WriteTo:0:Args:path", Defaults.FilePath<Program>()}
                    };

                    config.AddInMemoryCollection(configurationDefaults);
                })
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
