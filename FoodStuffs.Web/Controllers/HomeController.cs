using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
