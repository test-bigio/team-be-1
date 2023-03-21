using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BigioHrServices.Utilities;

namespace BigioHrServices.Db.Entities
{
    //[Table("employee")]
    public class Employee
    {
        public Employee()
        {
            var hasher = new Hasher();
            this.Password = hasher.HashDefaultPassword();
        }
        [Key]
        [Column("nik")]
        public string NIK { get; set; } = string.Empty;
        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column("sex")]
        public string Sex { get; set; } = string.Empty;
        [Required]
        [Column("join_date")]
        public DateOnly JoinDate { get; set; } = new DateOnly();
        [Required]
        [Column("work_length")]
        public string WorkLength { get; set; } = string.Empty;
        [Required]
        [Column("position")]
        public string Position { get; set; } = string.Empty;
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        [Required]
        [Column("password")]
        public string Password { get; set; }
        [Required]
        [Column("digital_signature")]
        public string DigitalSignature { get; set; } = "101010";

    }
}
