using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTFIX.Models
{
    [Table("kerusakan")]
    public class Kerusakan
    {
        [Key]
        public int IdKerusakan { get; set; }

        [Required]
        public string KodeKerusakan { get; set; }

        [Required]
        public string NamaKerusakan { get; set; }

        public string? Solusi { get; set; }
    }
}