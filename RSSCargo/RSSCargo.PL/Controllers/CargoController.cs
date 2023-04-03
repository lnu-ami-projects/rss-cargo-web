using Microsoft.AspNetCore.Mvc;

namespace RSSCargo.PL.Controllers
{
    public class CargoController : Controller
    {
        public IActionResult Cargo()
        {
            return View();
        }
    }
}
