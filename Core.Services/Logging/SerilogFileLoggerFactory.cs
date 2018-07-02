using Serilog;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Core.Services.Logging
{
    public class SerilogFileLoggerFactory
    {
        public static ILogger Create(string assemblyName, string environmentName)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var logPath = isWindows ? "C:/WebAppLogs" : "/webapplogs";
            var logFile = $"{logPath}/{assemblyName}-{environmentName}_.log";

            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(logFile,
                    rollingInterval : RollingInterval.Day,
                    retainedFileCountLimit : 15,
                    fileSizeLimitBytes : 10000000,
                    rollOnFileSizeLimit : true)
                .CreateLogger();
        }
    }
}
