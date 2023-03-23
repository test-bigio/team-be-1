using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Model.Notification;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BigioHrServices.Controllers
{
    [Route("notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
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
        public Pageable<NotificationResponse> GetNotificationsByEmployeeId([FromQuery] NotificationGetRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            request.Page = request.Page < 0 ? 0 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var uid = GetUid();
            return _notificationService.GetListNotificationByEmployeeId(request, uid);
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

            try
            {
                _notificationService.UpdateStatusNotification(id);
                return BaseResponse.Ok();
            } 
            catch (Exception ex)
            {
                return BaseResponse.FromException(ex);
            }
        }

        protected string GetUid()
        {
            string token = Request.Headers["Authorization"];
            token = token.Substring("Bearer ".Length);            
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var uid = tokenS.Claims.First(claim => claim.Type == "uid").Value;

            return uid;
        }
    }
}
