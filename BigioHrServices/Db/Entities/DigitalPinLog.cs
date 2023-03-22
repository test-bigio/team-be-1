using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BigioHrServices.Db.Entities
{
    public class DigitalPinLog
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Required]
        [Column("staff_id")]
        public string StaffId { get; set; }
        [Required]
        [Column("pin")]
        public string Pin { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
