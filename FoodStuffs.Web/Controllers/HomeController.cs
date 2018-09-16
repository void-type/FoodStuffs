using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("/forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
