using System.Security.Claims;
using System.Xml.Linq;

namespace BigioHrServices.Services
{
    public interface IAuthUserService
    {
        string Jwt { get; }
        string Name { get; }
        string Role { get; }
    }

    public class AuthUserService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public AuthUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Jwt { get; }
        //{
        //    get
        //    {
        //        if (_jwt == null || _jwt == string.Empty)
        //        {
        //            _jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //        }
        //        return _jwt;
        //    }
        //}

        public string Name
        {
            get
            {
                var identity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var claim = identity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                    if (claim != null)
                    {
                        return claim.Value;
                    }
                }

                return string.Empty;
            }
        }

        public string Role
        {
            get
            {
                var identity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var claim = identity.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                    if (claim != null)
                    {
                        return claim.Value;
                    }
                }

                return string.Empty;
            }
        }
    }
}
