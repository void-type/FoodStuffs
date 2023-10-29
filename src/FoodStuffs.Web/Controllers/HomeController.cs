using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers;

/// <summary>
/// Home controller.
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    private readonly ILogger _logger;

    /// <summary>
    /// Construct a new controller.
    /// </summary>
    /// <param name="logger"></param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Static error page.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [Route("/error")]
    public IActionResult Error()
    {
        _logger.LogInformation("Error page requested.");
        return View();
    }

    /// <summary>
    /// Static forbidden page.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [Route("/forbidden")]
    public IActionResult Forbidden()
    {
        _logger.LogInformation("Forbidden page requested.");
        return View();
    }

    /// <summary>
    /// Home page.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("Home page requested.");
        return View();
    }
}
