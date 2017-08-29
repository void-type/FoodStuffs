using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}