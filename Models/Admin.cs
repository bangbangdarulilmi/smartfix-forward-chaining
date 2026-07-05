using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMARTFIX.Models
{
    [Table("admin")]
    public class Admin
    {
        [Key]
        public int IdAdmin { get; set; }

        [Required(ErrorMessage = "Username wajib diisi")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        [StringLength(255)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nama wajib diisi")]
        [StringLength(100)]
        public string Nama { get; set; }
    }
}