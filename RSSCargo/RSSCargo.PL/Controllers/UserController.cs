using Microsoft.AspNetCore.Mvc;

namespace RSSCargo.PL.Controllers;

public class UserController : Controller
{
    public IActionResult Feeds()
    {
        return View();
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