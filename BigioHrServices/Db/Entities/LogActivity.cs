using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BigioHrServices.Db.Entities
{
    public class LogActivity
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("modul")]
        public string Modul { get; set; }
        [Column("activity")]
        public string Activity { get; set; }
    }
}
