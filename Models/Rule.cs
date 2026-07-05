using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTFIX.Models
{
    public class Rule
    {
        [Key]
        public int IdRule { get; set; }

        [Required]
        public int IdGejala { get; set; }

        [ForeignKey("IdGejala")]
        public Gejala? Gejala { get; set; }

        [Required]
        public int IdKerusakan { get; set; }

        [ForeignKey("IdKerusakan")]
        public Kerusakan? Kerusakan { get; set; }
    }
}