using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BigioHrServices.Utilities;

namespace BigioHrServices.Db.Entities
{
    //[Table("delegation")]
    public class Delegation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("nik")]
        public string NIK { get; set; } = string.Empty;
        [Required]
        [Column("parent_nik")]
        public string ParentNIK { get; set; } = string.Empty;
        [Required]
        [Column("priority")]
        public int Priority { get; set; }
    }
}
