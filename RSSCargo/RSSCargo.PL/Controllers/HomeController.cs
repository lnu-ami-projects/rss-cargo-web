using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

[AllowAnonymous]
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
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToAction("Feeds", "Rss");
        }

        var user = _userService.GetUserAuthenticated(HttpContext);
        _logger.LogInformation(user == null ? "No user logged in" : $"Logged in user: {user.UserName}");

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}