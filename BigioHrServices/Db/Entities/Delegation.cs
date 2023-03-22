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
        [Column("nik")]
        public string NIK { get; set; } = string.Empty;
        [Required]
        [Column("parent_nik")]
        public string ParentNIK { get; set; } = string.Empty;
    }
}
