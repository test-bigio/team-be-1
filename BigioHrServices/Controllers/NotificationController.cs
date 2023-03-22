using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Model.Notification;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    [Route("Notification")]
    [ApiController]
    public class NotificationController
    {
        private readonly INotificationService _notificationService;
        private readonly string RequestNull = "Request cannot be null!";

        public NotificationController(INotificationService NotificationService)
        {
            _notificationService = NotificationService;
        }

        [HttpGet("GetListNotification")]
        public DatatableResponse GetListNotification([FromQuery] NotificationGetRequest request)
        {
            if(request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _notificationService.GetListNotification(request);
        }

        [HttpGet("GetListNotificationByEmployeeId")]
        public DatatableResponse GetNotificationsByEmployeeId([FromQuery] NotificationGetRequest request, string nik)
        {
            if (request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _notificationService.GetListNotificationByEmployeeId(request, nik);
        }

        [HttpPut("UpdateStatusNotification")]
        public BaseResponse UpdateStatusNotification([FromBody] string id)
        {
            if (id == null) throw new Exception(RequestNull);

            _notificationService.UpdateStatusNotification(id);

            return new BaseResponse();
        }

        [HttpPut("UpdateStatusNotificationByEmployeeId")]
        public BaseResponse UpdateStatusNotificationByEmployeeId([FromBody] string id, string nik)
        {
            if (id == null) throw new Exception(RequestNull);

            _notificationService.UpdateStatusNotificationByEmployeeId(id, nik);

            return new BaseResponse();
        }
    }
}
