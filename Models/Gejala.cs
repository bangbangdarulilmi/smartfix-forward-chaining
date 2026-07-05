using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTFIX.Models
{
    [Table("gejala")]
    public class Gejala
    {
        [Key]
        public int IdGejala { get; set; }

        public string KodeGejala { get; set; } = "";

        public string NamaGejala { get; set; } = "";
    }
}