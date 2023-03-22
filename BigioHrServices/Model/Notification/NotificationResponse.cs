using System.ComponentModel.DataAnnotations.Schema;

namespace BigioHrServices.Model.Notification
{
    public class NotificationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool IsRead { get; set; }        
        public DateTime? ReadDate { get; set; }
    }
}
