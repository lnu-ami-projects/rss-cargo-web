using System.Diagnostics;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Email;
using RSSCargo.PL.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RSSCargo.PL.Models;

namespace RSSCargo.Tests;

public class AccountControllerTests
{
    private AccountController _accountController;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ILogger<AccountController>> _loggerMock;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<IEmailSender> _emailSenderMock;

    public AccountControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _loggerMock = new Mock<ILogger<AccountController>>();
        _mockUserManager = new Mock<UserManager<User>>(
        /* IUserStore<TUser> store */Mock.Of<IUserStore<User>>(),
        /* IOptions<IdentityOptions> optionsAccessor */null,
        /* IPasswordHasher<TUser> passwordHasher */null,
        /* IEnumerable<IUserValidator<TUser>> userValidators */null,
        /* IEnumerable<IPasswordValidator<TUser>> passwordValidators */null,
        /* ILookupNormalizer keyNormalizer */null,
        /* IdentityErrorDescriber errors */null,
        /* IServiceProvider services */null,
        /* ILogger<UserManager<TUser>> logger */null);

        _signInManagerMock = new Mock<SignInManager<User>>(
        _mockUserManager.Object,
        /* IHttpContextAccessor contextAccessor */Mock.Of<IHttpContextAccessor>(),
        /* IUserClaimsPrincipalFactory<TUser> claimsFactory */Mock.Of<IUserClaimsPrincipalFactory<User>>(),
        /* IOptions<IdentityOptions> optionsAccessor */null,
        /* ILogger<SignInManager<TUser>> logger */null,
        /* IAuthenticationSchemeProvider schemes */null,
        /* IUserConfirmation<TUser> confirmation */null);

        _emailSenderMock = new Mock<IEmailSender>();
    }

    [Fact]
    public async Task SignIn_ReturnsIActionResult()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Name, "Bill"),
            new (ClaimTypes.Email, "bill_gates@ukr.net"),
        }, "Cookies"));
        _userServiceMock.Setup(serv => serv.GetUserByEmail("bill_gates@ukr.net")).Returns((User?)null);
        _accountController = new AccountController(_loggerMock.Object, _userServiceMock.Object, _signInManagerMock.Object, _emailSenderMock.Object);
        _accountController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        var result = _accountController.SignIn(new SignInViewModel
        {
            Email = "bill_gates@ukr.net",
            Password = "1111"
        }, "/Account/SignIn");

        await Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public Task LogIn_ReturnsARedirectToActionResult()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
        }, "Cookies"));
        _accountController = new AccountController(_loggerMock.Object, _userServiceMock.Object, _signInManagerMock.Object, _emailSenderMock.Object);
        _accountController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        RedirectToActionResult result = (RedirectToActionResult)_accountController.Login();

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(null, result.ControllerName);
        Assert.Equal("SignIn", result.ActionName);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task SignUp_ReturnsCorrectView()
    {
        var returnUrl = "/Account/SignUp";
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
        }, "Cookies"));
        _accountController = new AccountController(_loggerMock.Object, _userServiceMock.Object, _signInManagerMock.Object, _emailSenderMock.Object);
        _accountController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        ViewResult result = (ViewResult)_accountController.SignUp(returnUrl);

        Assert.IsType<ViewResult>(result);
        Assert.Equal(returnUrl, result.ViewData["ReturnUrl"]);
    }
}

