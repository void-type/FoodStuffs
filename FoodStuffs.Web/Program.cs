using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace FoodStuffs.Web
{
  public class Program
  {
    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
            {
              var env = builderContext.HostingEnvironment;
              config.AddJsonFile($"appSecrets.{env.EnvironmentName}.json", true, true);
            })
            .UseStartup<Startup>()
            .UseSerilog()
            .Build();

    public static int Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Debug()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
          .MinimumLevel.Override("System", LogEventLevel.Warning)
          .Enrich.FromLogContext()
          .WriteTo.File("App_Data/Logs/log.txt", rollingInterval: RollingInterval.Day)
          .CreateLogger();

      try
      {
        Log.Information("Starting web host");
        BuildWebHost(args).Run();
        return 0;
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
        return 1;
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }
  }
}
