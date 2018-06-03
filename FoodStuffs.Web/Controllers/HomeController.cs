using Core.Services.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ApplicationSettings applicationSettings, IAntiforgery antiforgery)
        {
            _applicationSettings = applicationSettings;
            _antiforgery = antiforgery;
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            ViewBag.ApplicationName = _applicationSettings.Name;
            ViewBag.RequestVerificationToken = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;

            return View();
        }

        private readonly IAntiforgery _antiforgery;
        private readonly ApplicationSettings _applicationSettings;
    }
}