using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("User ID: " + HttpContext.Session.GetInt32("userId"));

        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}