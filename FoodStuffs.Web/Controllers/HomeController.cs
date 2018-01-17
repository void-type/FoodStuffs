using Core.Model.Services.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FoodStuffs.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IHostingEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly ILoggingService _logger;

    public HomeController(IHostingEnvironment environment, IConfiguration configuration, ILoggingService logger)
    {
      _environment = environment;
      _configuration = configuration;
      _logger = logger;
    }

    public IActionResult Index()
    {
      ViewBag.UseWebpackDevServer = _environment.IsEnvironment("Development");
      ViewBag.ApplicationName = _configuration["ApplicationName"];

      _logger.Info($"Environment: {_environment.EnvironmentName}");

      return View();
    }

    [Route("/Error")]
    public IActionResult Error()
    {
      return View();
    }
  }
}
