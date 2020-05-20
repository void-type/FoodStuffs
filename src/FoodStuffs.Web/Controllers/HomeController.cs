using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoidCore.Model.Logging;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILoggingService _logger;

        public HomeController(ILoggingService logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("/error")]
        public IActionResult Error()
        {
            _logger.Info("Error page requested.");
            return File("error.html", "text/html");
        }

        [AllowAnonymous]
        [Route("/forbidden")]
        public IActionResult Forbidden()
        {
            _logger.Info("Forbidden page requested.");
            return File("forbidden.html", "text/html");
        }

        [Route("/")]
        public IActionResult Index()
        {
            _logger.Info("Home page requested.");
            return File("app.html", "text/html");
        }
    }
}
