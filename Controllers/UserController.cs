using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================
        // HALAMAN DIAGNOSA USER
        // ==========================

        [HttpGet]
        public async Task<IActionResult> Diagnosa()
        {
            var gejala = await _context.Gejala
                .OrderBy(g => g.KodeGejala)
                .ToListAsync();

            return View(gejala);
        }

        // ==========================
        // PROSES DIAGNOSA USER
        // ==========================

        [HttpPost]
        public async Task<IActionResult> Diagnosa(List<int> gejalaDipilih)
        {
            var gejala = await _context.Gejala
                .OrderBy(g => g.KodeGejala)
                .ToListAsync();

            if (gejalaDipilih == null || gejalaDipilih.Count == 0)
            {
                ViewBag.Pesan = "Silakan pilih minimal satu gejala.";
                return View(gejala);
            }

            // Ambil semua rule beserta kerusakan
            var rules = await _context.Rule
                .Include(r => r.Kerusakan)
                .ToListAsync();

            Kerusakan? hasil = null;

            int nilaiTerbesar = 0;

            // Untuk menghitung persentase
            int totalRuleTerpilih = 0;
            int totalCocok = 0;

            // Kelompokkan berdasarkan kerusakan
            var kelompokRule = rules.GroupBy(r => r.IdKerusakan);

            foreach (var kelompok in kelompokRule)
            {
                var daftarGejala = kelompok
                    .Select(x => x.IdGejala)
                    .Distinct()
                    .ToList();

                int jumlahCocok = daftarGejala
                    .Count(x => gejalaDipilih.Contains(x));

                if (jumlahCocok > nilaiTerbesar)
                {
                    nilaiTerbesar = jumlahCocok;

                    hasil = kelompok.First().Kerusakan;

                    totalRuleTerpilih = daftarGejala.Count;

                    totalCocok = jumlahCocok;
                }
            }

            if (hasil == null)
            {
                ViewBag.Pesan = "Kerusakan tidak ditemukan.";
                return View(gejala);
            }

            int persen = 0;

            if (totalRuleTerpilih > 0)
            {
                persen = (int)Math.Round(
                    (double)totalCocok / totalRuleTerpilih * 100
                );
            }

            ViewBag.Persen = persen;

            ViewBag.Hasil = hasil;

            return View(gejala);
        }
    }
}