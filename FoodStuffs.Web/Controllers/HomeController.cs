using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Antiforgery;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IHostingEnvironment environment, IConfiguration configuration, IAntiforgery antiforgery)
        {
            _environment = environment;
            _configuration = configuration;
            _antiforgery = antiforgery;
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            ViewBag.ApplicationName = _configuration["ApplicationName"];
            ViewBag.RequestVerificationToken = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;

            return View();
        }

        private readonly IConfiguration _configuration;
        private readonly IAntiforgery _antiforgery;
        private readonly IHostingEnvironment _environment;
    }
}