using FoodStuffs.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStuffs.Web.Controllers;

/// <summary>
/// Home controller.
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
[Route("/")]
public partial class HomeController : Controller
{
    private readonly ILogger _logger;

    /// <summary>
    /// Construct a new controller.
    /// </summary>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Home page.
    /// </summary>
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        LogHomePage();
        return View();
    }

    /// <summary>
    /// Static error page.
    /// </summary>
    [AllowAnonymous]
    [Route("error/{statusCode}")]
    public IActionResult Error(int statusCode)
    {
        var model = statusCode switch
        {
            StatusCodes.Status403Forbidden => new ErrorViewModel
            {
                StatusCode = statusCode,
                Title = "Forbidden",
                Description = "You don't have access to this page."
            },
            StatusCodes.Status404NotFound => new ErrorViewModel
            {
                StatusCode = statusCode,
                Title = "Not found",
                Description = "This page was not found."
            },
            _ => new ErrorViewModel
            {
                StatusCode = statusCode,
                Title = $"Error",
                Description = "There was a problem getting the page you requested."
            },
        };

        LogErrorPage(statusCode);
        return View(model);
    }

    [LoggerMessage(0, LogLevel.Information, "Home page requested.")]
    private partial void LogHomePage();

    [LoggerMessage(0, LogLevel.Information, "Error page requested. StatusCode: {StatusCode}")]
    private partial void LogErrorPage(int statusCode);
}
