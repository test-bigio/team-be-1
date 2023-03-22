
using BigioHrServices.Model.Authentication;
using BigioHrServices.Model;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace BigioHrServices.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly string RequestNull = "Request cannot be null!";

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public LoginResponse AddEmployee([FromBody] LoginRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            return _authService.Login(request);
        }

        [HttpPost("reset_password")]
        public BaseResponse ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            _authService.ResetPassword(request);
            return new BaseResponse();
        }

        [HttpGet("me")]
        [Authorize]
        public string CheckToken()
        {
            // method to test the token
            var userData = HttpContext.User;
            return userData.ToJson();
        }
    }
}
