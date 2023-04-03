using Microsoft.AspNetCore.Mvc;

namespace RSSCargo.PL.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Feed()
        {
            return View();
        }
        public IActionResult AddFeed()
        {
            return View();
        }
    }
}
