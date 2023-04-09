using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;
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

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);
        
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        
        ViewData["UserFeeds"] = _rssFeedService.GetUserFeeds(user.Id).Select(f => new Tuple<int, string>(f.Id, f.Title));
        Console.WriteLine("OK");
    }

    public IActionResult Feeds()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        _logger.LogInformation($"Logged in user: {user.UserName}");

        if (Request.Query["feed-id"].Count == 0)
        {
            var userFeeds = _rssFeedService.GetUserFeeds(user.Id);
            var allFeedItems = userFeeds.SelectMany(feed => feed.Items);
            var sortedFeedItems = allFeedItems.OrderBy(item => DateTime.Parse(item.PublishDate));
            return View(new UserFeedsViewModel{UserFeedsItems = sortedFeedItems });
        }

        var feedId = int.Parse(Request.Query["feed-id"][0]);

        return View(new UserFeedsViewModel{UserFeed = _rssFeedService.GetUserFeed(user.Id, feedId)});
    }

    public IActionResult AddFeed()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        
        return View(new FeedsViewModel
        {
            UserFeeds = _rssFeedService.GetUserFeeds(user.Id)
        });
    }

    public IActionResult Cargo()
    {
        return View();
    }
}