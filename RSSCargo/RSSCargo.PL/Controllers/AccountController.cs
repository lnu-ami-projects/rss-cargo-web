using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace RSSCargo.PL.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult SignIn(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    private bool ValidateSignIn(string userName, string password)
    {
        // For this sample, all logins are successful.
        return true;
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(string userName, string password, string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;

        // Normally Identity handles sign in, but you can do it directly
        if (!ValidateSignIn(userName, password)) return View();
        
        var claims = new List<Claim>
        {
            new ("user", userName),
            new ("role", "Member")
        };

        await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

        return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");

    }

    // public IActionResult AccessDenied(string returnUrl = null)
    // {
    //     return View();
    // }

    public async Task<IActionResult> Signout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
    
    public IActionResult SignUp()
    {
        if (!Request.HasFormContentType)
        {
            return View();
        }
    
        var login = Request.Form["login"][0];
        var password = Request.Form["password"][0];
        if (login == null || password == null)
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
        int userId;
        try
        {
           // userId = _userService.LoginUser(login, password);
        }
        catch (Exception e)
        {
           // _logger.LogError("Login user " + e.Message);
           // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
       // _logger.LogInformation("Logged user: " + userId);
        //HttpContext.Session.SetInt32("userID", userId);
    
        return Redirect("/");
    }

}