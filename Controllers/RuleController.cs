using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class RuleController : Controller
    {
        private readonly AppDbContext _context;

        public RuleController(AppDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================

        public async Task<IActionResult> Index(string search)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rule = _context.Rule
                .Include(r => r.Gejala)
                .Include(r => r.Kerusakan)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                rule = rule.Where(r =>
                    r.Gejala!.NamaGejala.Contains(search) ||
                    r.Kerusakan!.NamaKerusakan.Contains(search));
            }

            ViewBag.Search = search;

            return View(await rule.ToListAsync());
        }

        // ================= CREATE =================

        public IActionResult Create()
        {
            ViewBag.Gejala = new SelectList(_context.Gejala, "IdGejala", "NamaGejala");
            ViewBag.Kerusakan = new SelectList(_context.Kerusakan, "IdKerusakan", "NamaKerusakan");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rule rule)
        {
            if (ModelState.IsValid)
            {
                _context.Rule.Add(rule);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Gejala = new SelectList(_context.Gejala, "IdGejala", "NamaGejala", rule.IdGejala);
            ViewBag.Kerusakan = new SelectList(_context.Kerusakan, "IdKerusakan", "NamaKerusakan", rule.IdKerusakan);

            return View(rule);
        }

        // ================= EDIT =================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rule = await _context.Rule.FindAsync(id);

            if (rule == null)
                return NotFound();

            ViewBag.Gejala = new SelectList(_context.Gejala, "IdGejala", "NamaGejala", rule.IdGejala);
            ViewBag.Kerusakan = new SelectList(_context.Kerusakan, "IdKerusakan", "NamaKerusakan", rule.IdKerusakan);

            return View(rule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rule rule)
        {
            if (id != rule.IdRule)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(rule);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Gejala = new SelectList(_context.Gejala, "IdGejala", "NamaGejala", rule.IdGejala);
            ViewBag.Kerusakan = new SelectList(_context.Kerusakan, "IdKerusakan", "NamaKerusakan", rule.IdKerusakan);

            return View(rule);
        }

        // ================= DELETE =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _context.Rule.FindAsync(id);

            if (rule != null)
            {
                _context.Rule.Remove(rule);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}