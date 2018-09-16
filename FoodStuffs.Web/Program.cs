using VoidCore.AspNet.Configuration;

namespace FoodStuffs.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return WebServer.BuildAndRun<Startup>(args);
        }
    }
}
