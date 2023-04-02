using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.DAL.Models;

namespace RSSCargo.PL.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(ILogger<AccountController> logger, IUserService userService, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _userService = userService;

        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpGet]
    public IActionResult SignIn(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> SignIn(string email, string password, string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var result = await _signInManager.PasswordSignInAsync(email, password, true, false);
        if (result.Succeeded)
        {
            // TODO: Fix
            // _signInManager.UserManager.Users.First(u => u.Id == 123);
            return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
        }

        return Redirect("/Account/SignIn");
    }

    [HttpGet]
    public IActionResult SignUp(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(string email, string username, string password, string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var user = new User()
        {
            Email = email,
            UserName = username,
        };

        var result = await _signInManager.UserManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // TODO: Fix
            // _signInManager.UserManager.Users.First(u => u.Id == 123);
            return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
        }

        return Redirect("/Account/SignUp");
    }
}