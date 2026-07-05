using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;

namespace SMARTFIX.Controllers
{
    public class RiwayatController : Controller
    {
        private readonly AppDbContext _context;

        public RiwayatController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // DAFTAR RIWAYAT
        // ============================
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var data = await _context.Diagnosa
                .Include(d => d.Kerusakan)
                .OrderByDescending(d => d.TanggalDiagnosa)
                .ToListAsync();

            return View(data);
        }

        // ============================
        // DETAIL RIWAYAT
        // ============================
        public async Task<IActionResult> Detail(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var diagnosa = await _context.Diagnosa
                .Include(d => d.Kerusakan)
                .FirstOrDefaultAsync(d => d.IdDiagnosa == id);

            if (diagnosa == null)
                return NotFound();

            var detail = await _context.DetailDiagnosa
                .Include(x => x.Gejala)
                .Where(x => x.IdDiagnosa == id)
                .ToListAsync();

            ViewBag.Detail = detail;

            return View(diagnosa);
        }
    }
}