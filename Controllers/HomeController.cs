using Microsoft.AspNetCore.Mvc;
using SMARTFIX.Data;
using SMARTFIX.Models;
using System.Diagnostics;

namespace SMARTFIX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Cek Login
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Nama Admin
            ViewBag.Nama = HttpContext.Session.GetString("Nama");

            // Total Data
            ViewBag.TotalGejala = _context.Gejala.Count();

            ViewBag.TotalKerusakan = _context.Kerusakan.Count();

            ViewBag.TotalRule = _context.Rule.Count();

            ViewBag.TotalAdmin = _context.Admin.Count();

            ViewBag.TotalRiwayat = _context.Diagnosa.Count();

            return View();
        }

        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}