using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FoodStuffs.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IHostingEnvironment _environment;
    private readonly IConfiguration _configuration;

    public HomeController(IHostingEnvironment environment, IConfiguration configuration)
    {
      _environment = environment;
      _configuration = configuration;
    }

    public IActionResult Index()
    {
      ViewBag.UseWebpackDevServer = _environment.IsEnvironment("Development");
      ViewBag.ApplicationName = _configuration["ApplicationName"];

      return View();
    }

    [Route("/Error")]
    public IActionResult Error()
    {
      return View();
    }
  }
}
