using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BigioHrServices.Utilities;

namespace BigioHrServices.Db.Entities
{
    public class Employee : BaseEntities
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
        public DateOnly JoinDate { get; set; }
        [Required]
        [Column("work_length")]
        public string WorkLength { get; set; } = string.Empty;
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Required]
        [Column("password")]
        public string Password { get; set; }
        [Required]
        [Column("digital_signature")]
        public string DigitalSignature { get; set; }
        [Column("jatah_cuti")]
        public int JatahCuti { get; set; } = 12;
        [Required]
        [Column("last_update_password")]
        public DateTime LastUpdatePassword { get; set; } = DateTime.UtcNow;
        [Required]
        [Column("is_on_leave")]
        public bool IsOnLeave { get; set; } = false;
        [Column("position_id")]
        [ForeignKey("Positions")]
        public string PositionCode { get; set; }
        [Required]
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("otp")]
        public string? OTP { get; set; }
    }
}
