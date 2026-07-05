using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTFIX.Models
{
    [Table("diagnosa")]
    public class Diagnosa
    {
        [Key]
        public int IdDiagnosa { get; set; }

        public string? NamaUser { get; set; }

        public DateTime TanggalDiagnosa { get; set; }

        public int IdKerusakan { get; set; }

        [ForeignKey("IdKerusakan")]
        public Kerusakan? Kerusakan { get; set; }

        public ICollection<DetailDiagnosa>? DetailDiagnosa { get; set; }
    }
}