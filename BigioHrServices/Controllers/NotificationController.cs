using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Model.Notification;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    [Route("notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController
    {
        private readonly INotificationService _notificationService;
        private readonly string RequestNull = "Request cannot be null!";

        public NotificationController(INotificationService NotificationService)
        {
            _notificationService = NotificationService;
        }

        [HttpGet]
        public Pageable<NotificationResponse> GetListNotification([FromQuery] NotificationGetRequest request)
        {
            if(request == null) throw new Exception(RequestNull);

            request.Page = request.Page < 0 ? 0 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _notificationService.GetListNotification(request);
        }

        [HttpGet("me")]
        public Pageable<NotificationResponse> GetNotificationsByEmployeeId([FromQuery] NotificationGetRequest request, string nik)
        {
            if (request == null) throw new Exception(RequestNull);

            request.Page = request.Page < 0 ? 0 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _notificationService.GetListNotificationByEmployeeId(request, nik);
        }

        [HttpGet("{id}")]
        public NotificationResponse GetDetailNotification(int id)
        {
            if (id == null) throw new Exception(RequestNull);

            return _notificationService.GetDetailNotification(id);
        }

        [HttpPut("update-status/{id}")]
        public BaseResponse UpdateStatusNotification(int id)
        {
            if (id == null) throw new Exception(RequestNull);

            _notificationService.UpdateStatusNotification(id);

            return new BaseResponse();
        }

        /*[HttpPost("create-notification")]
        public BaseResponse CreateNotification([FromQuery] NotificationAddRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            _notificationService.CreateNotification(request);

            return new BaseResponse();
        }

        [HttpDelete("delete-notification/{id}")]
        public BaseResponse DeleteNotification(int id)
        {
            if (id == null) throw new Exception(RequestNull);

            _notificationService.DeleteNotification(id);

            return new BaseResponse();
        }*/
    }
}
