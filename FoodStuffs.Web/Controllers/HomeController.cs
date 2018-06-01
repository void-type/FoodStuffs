using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration, IAntiforgery antiforgery)
        {
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

        private readonly IAntiforgery _antiforgery;
        private readonly IConfiguration _configuration;
    }
}