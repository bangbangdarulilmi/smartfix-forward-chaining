using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTFIX.Models
{
    [Table("detail_diagnosa")]
    public class DetailDiagnosa
    {
        [Key]
        public int IdDetail { get; set; }

        public int IdDiagnosa { get; set; }

        [ForeignKey("IdDiagnosa")]
        public Diagnosa? Diagnosa { get; set; }

        public int IdGejala { get; set; }

        [ForeignKey("IdGejala")]
        public Gejala? Gejala { get; set; }
    }
}