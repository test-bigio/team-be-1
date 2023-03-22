using System.ComponentModel.DataAnnotations.Schema;

namespace BigioHrServices.Model.Notification
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Nik { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool IsRead { get; set; }        
        public DateTime? ReadDate { get; set; }
    }
}
