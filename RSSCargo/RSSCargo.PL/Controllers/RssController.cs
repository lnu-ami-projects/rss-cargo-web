using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

[Authorize]
public class RssController : Controller
{
    private readonly ILogger<RssController> _logger;
    private readonly IUserService _userService;
    private readonly IRssFeedService _rssFeedService;

    public RssController(ILogger<RssController> logger, IUserService userService, IRssFeedService rssFeedService)
    {
        _logger = logger;
        _userService = userService;
        _rssFeedService = rssFeedService;
    }

    public IActionResult Feeds()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        _logger.LogInformation($"Logged in user: {user.UserName}");
        
        var model = new FeedsViewModel(_rssFeedService.GetUserFeeds(user.Id));

        if (Request.Query["feed-id"].Count == 0) return View(model);

        var feedId = int.Parse(Request.Query["feed-id"][0]);
        model.UserFeed = _rssFeedService.GetUserFeed(user.Id, feedId);

        return View(model);
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