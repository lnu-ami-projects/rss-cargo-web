using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.DAL.Models;
using Serilog;

namespace RSSCargo.PL.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService;

    public AccountController(ILogger<AccountController> logger, IUserService userService,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _userService = userService;
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

        var user = _userService.GetUserByEmail(email);
        if (user == null) return RedirectToAction("SignIn","Account");

        var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, false);
        if (!result.Succeeded)
        {
            _logger.LogError("Sign in: " + result);
            return RedirectToAction("SignIn","Account");
        }
        
        _userService.UserAuthenticated(HttpContext, user.Id);
        return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/Rss/Feeds");
    } 

    [HttpGet]
    public IActionResult SignUp(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(string email, string username, string password, string cpassword)
    {
        if (password != cpassword)
        {
            return RedirectToAction("SignUp","Account");
        }
        var user = new User
        {
            Email = email,
            UserName = username,
        };

        var result = await _signInManager.UserManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            _logger.LogError("Sign up: " + result);
            return RedirectToAction("SignUp","Account");
        }

        var createdUser = _userService.GetUserByEmail(email);
        _userService.UserAuthenticated(HttpContext, createdUser!.Id);
        return RedirectToAction("SignIn","Account");
    }

    public async Task<IActionResult> UserSignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (HttpContext.Request.Cookies.Count <= 0) RedirectToAction("Index", "Home");

        var siteCookies = HttpContext.Request.Cookies
            .Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));

        foreach (var cookie in siteCookies)
        {
            Response.Cookies.Delete(cookie.Key);
        }

        return RedirectToAction("Index", "Home");
    }
}