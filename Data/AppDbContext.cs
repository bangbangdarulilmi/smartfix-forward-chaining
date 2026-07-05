using Microsoft.EntityFrameworkCore;
using SMARTFIX.Models;

namespace SMARTFIX.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<Gejala> Gejala { get; set; }

        public DbSet<Kerusakan> Kerusakan { get; set; }

        public DbSet<Rule> Rule { get; set; }

        public DbSet<Diagnosa> Diagnosa { get; set; }

        public DbSet<DetailDiagnosa> DetailDiagnosa { get; set; }
    }
}