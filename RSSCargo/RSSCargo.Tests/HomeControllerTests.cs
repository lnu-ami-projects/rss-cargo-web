using System.Diagnostics;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.PL.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RSSCargo.Tests;

public class HomeControllerTests
{
    private HomeController _homeController;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ILogger<HomeController>> _loggerMock;

    public HomeControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _loggerMock = new Mock<ILogger<HomeController>>();
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithoutRedirect()
    {
        var user = new ClaimsPrincipal();
        _homeController = new HomeController(_loggerMock.Object, _userServiceMock.Object);
        _homeController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        ViewResult result = (ViewResult)_homeController.Index();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<IActionResult>(result);
        Assert.IsType<ViewResult>(result);
        Assert.True(string.IsNullOrEmpty(result.ViewName));
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithALog()
    {
        int expectedloggerInvocationCount = 1;
        var user = new ClaimsPrincipal();
        _homeController = new HomeController(_loggerMock.Object, _userServiceMock.Object);
        _homeController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        ViewResult result = (ViewResult)_homeController.Index();

        Assert.IsType<ViewResult>(result);
        Assert.Equal(expectedloggerInvocationCount, _loggerMock.Invocations.Count);
        Assert.Equal(LogLevel.Information, _loggerMock.Invocations[0].Arguments[0]);
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithGetUserAuthenticatedInvocation()
    {
        int expecteduserServiceInvocationCount = 1;
        var user = new ClaimsPrincipal();
        _userServiceMock.Setup(serv => serv.GetUserAuthenticated(new DefaultHttpContext())).Returns((User)null);
        _homeController = new HomeController(_loggerMock.Object, _userServiceMock.Object);
        _homeController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        ViewResult result = (ViewResult)_homeController.Index();

        Assert.IsType<ViewResult>(result);
        Assert.Equal(expecteduserServiceInvocationCount, _userServiceMock.Invocations.Count);
    }

    [Fact]
    public async Task Index_ReturnsARedirectToActionResult_WithAuthenticatedUser()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "Cookies"));
        _homeController = new HomeController(_loggerMock.Object, _userServiceMock.Object);
        _homeController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        RedirectToActionResult result = (RedirectToActionResult)_homeController.Index();

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Rss", result.ControllerName);
        Assert.Equal("Feeds", result.ActionName);
    }
}

