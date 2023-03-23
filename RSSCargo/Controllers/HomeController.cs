using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.PL.Models;

namespace RSSCargo.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGenericRepository<User> _genericRepository;

    public HomeController(ILogger<HomeController> logger, IGenericRepository<User> genericRepository)
    {
        _logger = logger;
        _genericRepository = genericRepository;
    }

    public IActionResult Index()
    {
        var user = _genericRepository.GetUserByEmail("a");
        Console.WriteLine(user.Username);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}