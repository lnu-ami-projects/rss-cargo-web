using System.Diagnostics;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;
using RSSCargo.PL.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using RSSCargo.PL.Models;

namespace RSSCargo.Tests;

public class RssControllerTests
{
    private readonly Mock<ILogger<RssController>> _loggerMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IUserFeedService> _userFeedServiceMock;
    private readonly Mock<IUserCargoService> _userCargoServiceMock;
    private readonly Mock<ICargoService> _cargoServiceMock;
    private readonly Mock<IRssFeedService> _rssFeedServiceMock;
    private RssController _rssController;

    public RssControllerTests()
    {
        _loggerMock = new Mock<ILogger<RssController>>();
        _userServiceMock = new Mock<IUserService>();
        _userFeedServiceMock = new Mock<IUserFeedService>();
        _userCargoServiceMock = new Mock<IUserCargoService>();
        _cargoServiceMock = new Mock<ICargoService>();
        _rssFeedServiceMock = new Mock<IRssFeedService>();
    }

    [Fact]
    public async Task AddFeed_ReturnsViewResult_WithUserFeeds()
    {
        var userId = 1;
        var feedFirst = new RssFeed(111, "http://rss.cnn.com/rss/edition_world.rss");
        var feedSecond = new RssFeed(222, "http://rss.cnn.com/rss/edition_business.rss");
        var user = new User { Id = userId };
        var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "Cookies"));
        var httpContext = new DefaultHttpContext() { User = userClaimPrincipal };
        _userServiceMock.Setup(serv => serv.GetUserAuthenticated(httpContext)).Returns(user);
        _rssFeedServiceMock.Setup(serv => serv.GetUserFeeds(1)).Returns(new List<RssFeed> { feedFirst, feedSecond });
        _rssController = new RssController(_loggerMock.Object, _userServiceMock.Object,
            _userFeedServiceMock.Object, _userCargoServiceMock.Object, _cargoServiceMock.Object, _rssFeedServiceMock.Object);
        _rssController.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        ViewResult result = (ViewResult)_rssController.AddFeed();

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<FeedsViewModel>(
            viewResult.ViewData.Model);
    }

    [Fact]
    public async Task RemoveFeed_RedirectsToAddFeedViewResult_WithUserAuthenticated()
    {
        var userId = 1;
        var feedFirst = new RssFeed(111, "http://rss.cnn.com/rss/edition_world.rss");
        var user = new User { Id = userId };
        var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "Cookies"));
        var httpContext = new DefaultHttpContext() { User = userClaimPrincipal };
        _userServiceMock.Setup(serv => serv.GetUserAuthenticated(httpContext)).Returns(user);
        _userFeedServiceMock.Setup(serv => serv.RemoveUserFeed(userId, feedFirst.Link));
        _rssController = new RssController(_loggerMock.Object, _userServiceMock.Object,
            _userFeedServiceMock.Object, _userCargoServiceMock.Object, _cargoServiceMock.Object, _rssFeedServiceMock.Object);
        _rssController.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        RedirectToActionResult result = (RedirectToActionResult)_rssController.RemoveFeed(feedFirst.Link);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(result.ControllerName);
        Assert.Equal("AddFeed", result.ActionName);
    }

    [Fact]
    public async Task AddFeed_RedirectsToAddFeedViewResult_ViaUrlUnsuccessful()
    {
        var feedFirst = new RssFeed(111, "http://rss.cnn.com/rss/edition_world.rss");
        _rssFeedServiceMock.Setup(serv => serv.ValidateFeed(feedFirst.Link)).Returns(false);
        _rssController = new RssController(_loggerMock.Object, _userServiceMock.Object,
            _userFeedServiceMock.Object, _userCargoServiceMock.Object, _cargoServiceMock.Object, _rssFeedServiceMock.Object);

        RedirectToActionResult result = (RedirectToActionResult)_rssController.AddFeed(feedFirst.Link);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(result.ControllerName);
        Assert.Equal("AddFeed", result.ActionName);
    }

    [Fact]
    public async Task AddFeed_RedirectsToAddFeedViewResult_ViaUrlSuccessful()
    {
        var userId = 1;
        var feedFirst = new RssFeed(111, "http://rss.cnn.com/rss/edition_world.rss");
        var user = new User { Id = userId };
        var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "Cookies"));
        var httpContext = new DefaultHttpContext() { User = userClaimPrincipal };
        _userServiceMock.Setup(serv => serv.GetUserAuthenticated(httpContext)).Returns(user);
        _userFeedServiceMock.Setup(serv => serv.AddUserFeed(userId, feedFirst.Link));
        _rssFeedServiceMock.Setup(serv => serv.ValidateFeed(feedFirst.Link)).Returns(true);
        _rssController = new RssController(_loggerMock.Object, _userServiceMock.Object,
            _userFeedServiceMock.Object, _userCargoServiceMock.Object, _cargoServiceMock.Object, _rssFeedServiceMock.Object);
        _rssController.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };

        RedirectToActionResult result = (RedirectToActionResult)_rssController.AddFeed(feedFirst.Link);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Null(result.ControllerName);
        Assert.Equal("AddFeed", result.ActionName);
    }
}

