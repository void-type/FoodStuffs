using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IHostingEnvironment _env;

    public HomeController(IHostingEnvironment env)
    {
      _env = env;
    }

    public IActionResult Index()
    {
      ViewBag.IsDevelopment = _env.IsDevelopment();

      return View();
    }
  }
}
