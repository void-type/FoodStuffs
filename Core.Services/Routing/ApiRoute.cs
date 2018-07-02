using Microsoft.AspNetCore.Mvc;

namespace Core.Services.Routing
{
    public class ApiRoute : RouteAttribute
    {
        public static string BasePath => "/api";

        public ApiRoute(string path) : base($"{BasePath}/{path}")
        { }
    }
}
