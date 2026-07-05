using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // INDEX
        // ============================
        public async Task<IActionResult> Index(string? search)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Index", "Login");

            var data = _context.Admin.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x =>
                    x.Nama.Contains(search) ||
                    x.Username.Contains(search));
            }

            ViewBag.Search = search;

            return View(await data.ToListAsync());
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Admin.Add(admin);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(admin);
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
                return RedirectToAction("Index", "Login");

            var admin = await _context.Admin.FindAsync(id);

            if (admin == null)
                return NotFound();

            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Admin admin)
        {
            if (id != admin.IdAdmin)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(admin);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(admin);
        }

        // ============================
        // DELETE
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var admin = await _context.Admin.FindAsync(id);

            if (admin != null)
            {
                _context.Admin.Remove(admin);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}