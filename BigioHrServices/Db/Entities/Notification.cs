using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigioHrServices.Db.Entities
{
    public class Notification
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("nik")]
        [ForeignKey("Employee")]
        public string Nik { get; set; }

        [MaxLength(255)]
        [Required]
        [Column("title")]
        public string Title { get; set; }

        [MaxLength(255)]
        [Column("body")]
        public string Body { get; set; }

        [MaxLength(8000)]
        [Column("data")]
        public string Data { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("read_date")]
        public DateTime? ReadDate { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
