using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Notification;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    public class NotificationController
    {
        private readonly INotificationService _NotificationService;
        private readonly string RequestNull = "Request cannot be null!";

        public NotificationController(INotificationService NotificationService)
        {
            _NotificationService = NotificationService;
        }

        [HttpGet("GetListNotification")]
        public DatatableResponse GetNotifications([FromQuery] NotificationGetRequest request)
        {
            if(request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _NotificationService.GetList(request);
        }
    }
}
