using Microsoft.AspNetCore.Mvc;
using RSSCargo.BLL.Services.Contracts;

namespace RSSCargo.PL.Controllers;

public class RssController : Controller
{
    private readonly ILogger<RssController> _logger;
    private readonly IUserService _userService;

    public RssController(ILogger<RssController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public IActionResult Feeds()
    {
        var user = _userService.GetUserAuthenticated(HttpContext);
        _logger.LogInformation(user == null ? "No user logged in" : $"Logged in user: {user.UserName}");
        return View();
    }
    public IActionResult AddFeed()
    {
        return View();
    }
    
    public IActionResult Cargo()
    {
        return View();
    }
}