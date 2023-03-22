using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BigioHrServices.Db.Entities
{
    public class Position
    {
        [Key]
        [Column("code")]
        public string Code { get; set; } = string.Empty;
        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column("level")]
        public string Level { get; set; } = string.Empty;
        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

    }
}
