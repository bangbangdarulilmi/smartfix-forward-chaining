using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class GejalaController : Controller
    {
        private readonly AppDbContext _context;

        public GejalaController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // TAMPIL DATA + PENCARIAN
        // =========================
        public async Task<IActionResult> Index(string search)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var data = _context.Gejala.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x =>
                    x.KodeGejala.Contains(search) ||
                    x.NamaGejala.Contains(search));
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
        public async Task<IActionResult> Create(Gejala gejala)
        {
            if (ModelState.IsValid)
            {
                _context.Gejala.Add(gejala);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gejala);
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

            var gejala = await _context.Gejala.FindAsync(id);

            if (gejala == null)
            {
                return NotFound();
            }

            return View(gejala);
        }

        // =========================
        // UPDATE DATA
        // =========================
        [HttpPost]
        public async Task<IActionResult> Edit(Gejala gejala)
        {
            if (ModelState.IsValid)
            {
                _context.Gejala.Update(gejala);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gejala);
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

            var gejala = await _context.Gejala.FindAsync(id);

            if (gejala != null)
            {
                _context.Gejala.Remove(gejala);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}