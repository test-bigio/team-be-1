using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.Notification
{
    public class NotificationAddRequest
    {
        public int Id { get; set; }
        public string Nik { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
