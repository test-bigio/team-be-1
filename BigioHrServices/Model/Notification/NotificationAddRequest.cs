using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.Notification
{
    public class NotificationAddRequest
    {
        public string Nik { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
