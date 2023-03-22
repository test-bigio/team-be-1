using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Model.Notification;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    [Route("notification")]
    [ApiController]
    public class NotificationController
    {
        private readonly INotificationService _notificationService;
        private readonly string RequestNull = "Request cannot be null!";

        public NotificationController(INotificationService NotificationService)
        {
            _notificationService = NotificationService;
        }

        [HttpGet("get-list-notification")]
        public DatatableResponse GetListNotification([FromQuery] NotificationGetRequest request)
        {
            if(request == null) throw new Exception(RequestNull);

            request.Page = request.Page < 0 ? 0 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _notificationService.GetListNotification(request);
        }

        [HttpGet("get-list-notification-by-employee-id")]
        public DatatableResponse GetNotificationsByEmployeeId([FromQuery] NotificationGetRequest request, string nik)
        {
            if (request == null) throw new Exception(RequestNull);

            request.Page = request.Page < 0 ? 0 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _notificationService.GetListNotificationByEmployeeId(request, nik);
        }

        [HttpGet("get-detail-notification/{id}")]
        public NotificationResponse GetDetailNotification(string id)
        {
            if (id == null) throw new Exception(RequestNull);

            return _notificationService.GetDetailNotification(id);
        }

        [HttpPut("update-status-notification/{id}")]
        public BaseResponse UpdateStatusNotification(string id)
        {
            if (id == null) throw new Exception(RequestNull);

            _notificationService.UpdateStatusNotification(id);

            return new BaseResponse();
        }
    }
}
