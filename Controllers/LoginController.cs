using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // ================= LOGIN =================

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Admin model)
        {
            var admin = _context.Admin.FirstOrDefault(a =>
                a.Username == model.Username &&
                a.Password == model.Password);

            if (admin != null)
            {
                HttpContext.Session.SetString("Admin", admin.Username);
                HttpContext.Session.SetString("Nama", admin.Nama);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Username atau Password salah!";
            return View();
        }

        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            // Hapus seluruh session
            HttpContext.Session.Clear();

            // Kembali ke Landing Page
            return RedirectToAction("Index", "Landing");
        }
    }
}