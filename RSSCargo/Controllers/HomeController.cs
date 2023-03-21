using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Index page requested.");

        try
        {
            for (int i = 0; i < 100; i++)
            {
                if (i == 50)
                {
                    throw new Exception("This is exception for Index page.");
                }
                else
                {
                    _logger.LogInformation("The value of i is {LoopVariable}", i);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception caught in Index page call.");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}