using System.ComponentModel.DataAnnotations;

namespace BigioHrServices.Db.Entities
{
    public class AuditModul
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string IpAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Activity { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
