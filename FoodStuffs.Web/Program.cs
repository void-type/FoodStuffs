using VoidCore.AspNet.Configuration;
using VoidCore.AspNet.Logging;

namespace FoodStuffs.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // TODO: MS warnings are suppressed in logs.
            var logger = SerilogFileLoggerFactory.Create<Startup>(true);
            return WebServer.BuildAndRun<Startup>(args, logger);
        }
    }
}
