using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigioHrServices.Db.Entities
{
    public class Notification
    {
        [Column("id")]
        public string Id { get; set; }

        /*[Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }*/

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

        //public virtual User User { get; set; }

        /// <summary>
        /// OBSOLETE, NO USE ANYMORE, DEFAULT VALUE
        /// 
        /// Real type of the notification, approve, reject, etc.
        /// </summary>
        //public NotificationConstantType ConstantType { get; set; }
    }
}
