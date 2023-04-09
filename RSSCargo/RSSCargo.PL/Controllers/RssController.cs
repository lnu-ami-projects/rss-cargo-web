using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

[Authorize]
public class RssController : Controller
{
    private readonly ILogger<RssController> _logger;
    private readonly IUserService _userService;
    private readonly IUserFeedService _userFeedService;
    private readonly IRssFeedService _rssFeedService;

    public RssController(ILogger<RssController> logger, IUserService userService, IUserFeedService userFeedService,IRssFeedService rssFeedService)
    {
        _logger = logger;
        _userService = userService;
        _userFeedService = userFeedService;
        _rssFeedService = rssFeedService;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);
        
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        
        ViewData["UserFeeds"] = _rssFeedService.GetUserFeeds(user.Id).Select(f => new Tuple<int, string>(f.Id, f.Title));
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

    [HttpGet]
    public IActionResult AddFeed()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        
        return View(new FeedsViewModel
        {
            UserFeeds = _rssFeedService.GetUserFeeds(user.Id)
        });
    }
    
    [HttpPost]
    public IActionResult AddFeed(string feedUrl)
    {
        if (!_rssFeedService.ValidateFeed(feedUrl))
        {
            return RedirectToAction("AddFeed");
        }
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        _userFeedService.AddUserFeed(user.Id, feedUrl);
        return RedirectToAction("AddFeed");
    }

    [HttpPost]
    public IActionResult RemoveFeed(string feedUrl)
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        _userFeedService.RemoveUserFeed(user.Id, feedUrl);
        return RedirectToAction("AddFeed");
    }

    public IActionResult Cargo()
    {
        return View();
    }
}