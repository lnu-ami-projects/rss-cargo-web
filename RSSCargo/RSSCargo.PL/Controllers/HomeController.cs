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

    public IActionResult Login()
    {
        if (!Request.HasFormContentType)
        {
            return View();
        }

        var login = Request.Form["login"][0];
        var password = Request.Form["password"][0];
        if (login == null || password == null)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        int userId;
        try
        {
            userId = _userService.LoginUser(login, password);
        }
        catch (Exception e)
        {
            _logger.LogError("Login user " + e.Message);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        _logger.LogInformation("Logged user: " + userId);
        HttpContext.Session.SetInt32("userID", userId);

        return Redirect("/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}