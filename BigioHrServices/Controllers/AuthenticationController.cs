
using BigioHrServices.Model.Authentication;
using BigioHrServices.Model;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    public class AuthenticationController
    {
        private readonly IAuthenticationService _authService;
        private readonly string RequestNull = "Request cannot be null!";

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("auth/login")]
        public LoginResponse AddEmployee([FromBody] LoginRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            return _authService.Login(request);
        }

        [HttpPost("auth/reset_password")]
        public BaseResponse ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            _authService.ResetPassword(request);
            return new BaseResponse();
        }

    }
}
