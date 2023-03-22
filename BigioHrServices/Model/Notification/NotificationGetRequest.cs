using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.Notification
{
    public class NotificationGetRequest : DatatableRequest
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
