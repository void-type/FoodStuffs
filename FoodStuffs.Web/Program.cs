using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Runtime.InteropServices;

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
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var logPath = isWindows ? "C:/WebAppLogs/" : "/webapplogs/";
            var logFile = $"{logPath}FoodStuffs-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}_.log";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(logFile,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 15,
                    fileSizeLimitBytes: 10000000,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");
                BuildWebHost(args).Run();
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