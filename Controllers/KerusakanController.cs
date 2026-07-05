using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class KerusakanController : Controller
    {
        private readonly AppDbContext _context;

        public KerusakanController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // TAMPIL DATA
        // =========================
        public async Task<IActionResult> Index(string search)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var data = _context.Kerusakan.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x =>
                    x.KodeKerusakan.Contains(search) ||
                    x.NamaKerusakan.Contains(search));
            }

            ViewBag.Search = search;

            return View(await data.ToListAsync());
        }

        // =========================
        // FORM TAMBAH
        // =========================
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // =========================
        // SIMPAN DATA
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kerusakan kerusakan)
        {
            if (ModelState.IsValid)
            {
                _context.Kerusakan.Add(kerusakan);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(kerusakan);
        }

        // =========================
        // FORM EDIT
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var kerusakan = await _context.Kerusakan.FindAsync(id);

            if (kerusakan == null)
            {
                return NotFound();
            }

            return View(kerusakan);
        }

        // =========================
        // UPDATE DATA
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kerusakan kerusakan)
        {
            if (id != kerusakan.IdKerusakan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(kerusakan);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(kerusakan);
        }

        // =========================
        // HAPUS DATA
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var kerusakan = await _context.Kerusakan.FindAsync(id);

            if (kerusakan != null)
            {
                _context.Kerusakan.Remove(kerusakan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}