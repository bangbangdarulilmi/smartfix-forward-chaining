using Microsoft.AspNetCore.Mvc;

namespace SMARTFIX.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}