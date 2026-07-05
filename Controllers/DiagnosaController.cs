using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMARTFIX.Data;
using SMARTFIX.Models;

namespace SMARTFIX.Controllers
{
    public class DiagnosaController : Controller
    {
        private readonly AppDbContext _context;

        public DiagnosaController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // TAMPIL HALAMAN DIAGNOSA
        // =========================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var gejala = await _context.Gejala
                .OrderBy(g => g.KodeGejala)
                .ToListAsync();

            return View(gejala);
        }

        // =========================
        // PROSES DIAGNOSA
        // =========================
        [HttpPost]
        public async Task<IActionResult> Index(List<int> gejalaDipilih)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var gejala = await _context.Gejala
                .OrderBy(g => g.KodeGejala)
                .ToListAsync();

            if (gejalaDipilih == null || gejalaDipilih.Count == 0)
            {
                ViewBag.Pesan = "Silakan pilih minimal satu gejala.";
                return View(gejala);
            }

            // Debug
            ViewBag.Debug = string.Join(", ", gejalaDipilih);

            // Ambil semua rule beserta kerusakan
            var rules = await _context.Rule
                .Include(r => r.Kerusakan)
                .ToListAsync();

            Kerusakan? hasil = null;

            int nilaiTerbesar = 0;

            // Kelompokkan berdasarkan kerusakan
            var kelompokRule = rules.GroupBy(r => r.IdKerusakan);

            foreach (var kelompok in kelompokRule)
            {
                var daftarGejala = kelompok
                    .Select(x => x.IdGejala)
                    .Distinct()
                    .ToList();

                // Hitung jumlah gejala yang cocok
                int jumlahCocok = daftarGejala
                    .Count(x => gejalaDipilih.Contains(x));

                // Ambil yang paling banyak cocok
                if (jumlahCocok > nilaiTerbesar)
                {
                    nilaiTerbesar = jumlahCocok;
                    hasil = kelompok.First().Kerusakan;
                }
            }

            ViewBag.Hasil = hasil;

            if (hasil == null)
            {
                ViewBag.Pesan = "Kerusakan tidak ditemukan.";
                return View(gejala);
            }

            // =========================
            // SIMPAN KE TABEL DIAGNOSA
            // =========================

            var diagnosa = new Diagnosa
            {
                NamaUser = HttpContext.Session.GetString("Nama"),
                TanggalDiagnosa = DateTime.Now,
                IdKerusakan = hasil.IdKerusakan
            };

            _context.Diagnosa.Add(diagnosa);
            await _context.SaveChangesAsync();

            // =========================
            // SIMPAN DETAIL DIAGNOSA
            // =========================

            foreach (var id in gejalaDipilih)
            {
                var detail = new DetailDiagnosa
                {
                    IdDiagnosa = diagnosa.IdDiagnosa,
                    IdGejala = id
                };

                _context.DetailDiagnosa.Add(detail);
            }

            await _context.SaveChangesAsync();

            return View(gejala);
        }
    }
}