using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/error")]
        public IActionResult Error()
        {
            _logger.LogInformation("Error page requested.");
            return File("error.html", "text/html");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/forbidden")]
        public IActionResult Forbidden()
        {
            _logger.LogInformation("Forbidden page requested.");
            return File("forbidden.html", "text/html");
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Home page requested.");
            return File("app.html", "text/html");
        }
    }
}
