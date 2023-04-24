using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;
using RSSCargo.DAL.Models;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

[Authorize]
public class RssController : Controller
{
    private readonly ILogger<RssController> _logger;
    private readonly IUserService _userService;
    private readonly IUserFeedService _userFeedService;
    private readonly IUserCargoService _userCargoService;
    private readonly ICargoService _cargoService;
    private readonly IRssFeedService _rssFeedService;

    public RssController(ILogger<RssController> logger, IUserService userService, IUserFeedService userFeedService,
        IUserCargoService userCargoService, ICargoService cargoService, IRssFeedService rssFeedService)
    {
        _logger = logger;
        _userService = userService;
        _userFeedService = userFeedService;
        _userCargoService = userCargoService;
        _cargoService = cargoService;
        _rssFeedService = rssFeedService;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);

        var user = _userService.GetUserAuthenticated(HttpContext)!;

        ViewData["UserFeeds"] = _rssFeedService
            .GetUserFeeds(user.Id)
            .Select(f => new Tuple<int, string>(f.Id, f.Title));

        ViewData["UserCargos"] = _userCargoService
            .GetUserCargos(user.Id).ToList()
            .Select(uc => new Tuple<int, string>(uc.CargoId, _cargoService.GetCargoById(uc.CargoId).Name));
    }

    public IActionResult Feeds()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        _logger.LogInformation($"Logged in user: {user.UserName}");

        if (Request.Query["feed-id"].Count == 0)
        {
            var userFeeds = _rssFeedService.GetUserFeeds(user.Id);
            var allFeedItems = userFeeds.SelectMany(feed => feed.Items);
            var sortedFeedItems = allFeedItems.OrderByDescending(item => DateTime.Parse(item.PublishDate));
            return View(new UserFeedsViewModel { UserFeedsItems = sortedFeedItems });
        }

        var feedId = int.Parse(Request.Query["feed-id"][0]);

        return View(new UserFeedsViewModel { UserFeed = _rssFeedService.GetUserFeed(user.Id, feedId) });
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

    public IActionResult YourCargos()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;

        var subCargos = _cargoService.GetSubscribedCargos(user.Id).ToArray();
        var unsubCargos = _cargoService.GetUnsubscribedCargos(user.Id).ToArray();

        var cargoFeeds = new Dictionary<int, List<string>>();

        foreach (var cargo in subCargos.Concat(unsubCargos))
        {
            var feeds = _cargoService.GetCargoFeeds(cargo.Id)
                .Select(cargoCargoFeed => new RssFeed(0, cargoCargoFeed.RssFeed).Title)
                .ToList();
            cargoFeeds[cargo.Id] = feeds;
        }

        return View(new YourCargosViewModel
        {
            SubCargos = subCargos,
            UnsubCargos = unsubCargos,
            CargoFeeds = cargoFeeds
        });
    }

    public IActionResult SubToCargo()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        var cargoId = int.Parse(Request.Query["cargo-id"][0]!);

        _userCargoService.SubscribeUserCargo(user.Id, cargoId);

        return RedirectToAction("YourCargos");
    }

    public IActionResult UnsubCargo()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        var cargoId = int.Parse(Request.Query["cargo-id"][0]!);

        _userCargoService.UnsubscribeUserCargo(user.Id, cargoId);

        return RedirectToAction("YourCargos");
    }

    public IActionResult CargoFeeds()
    {
        var user = _userService.GetUserAuthenticated(HttpContext)!;
        _logger.LogInformation($"Logged in user: {user.UserName}");

        if (Request.Query["cargo-id"].Count == 0)
        {
            var userCargos = _userCargoService.GetUserCargos(user.Id).ToList();

            var cargoAllFeeds = new List<RssFeed>();
            foreach (var cargoFeeds in userCargos.Select(userCargo => _cargoService.GetRssCargoFeeds(userCargo.CargoId)))
            {
                cargoAllFeeds.AddRange(cargoFeeds);
            }

            return View(new CargoFeedsViewModel
            {
                CargoName = "",
                UserCargoFeeds = cargoAllFeeds,
            });
        }

        var cargoId = int.Parse(Request.Query["cargo-id"][0]);

        return View(new CargoFeedsViewModel
        {
            CargoName = _cargoService.GetCargoById(cargoId).Name,
            UserCargoFeeds = _cargoService.GetRssCargoFeeds(cargoId)
        });
    }
}