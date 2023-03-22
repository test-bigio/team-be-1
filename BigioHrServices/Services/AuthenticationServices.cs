using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Authentication;
using BigioHrServices.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace BigioHrServices.Services
{
    public interface IAuthenticationService
    {
        public LoginResponse Login(LoginRequest request);
    }
    public class AuthenticationServices : IAuthenticationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AuthenticationServices(IConfiguration config, ApplicationDbContext db)
        {
            _config = config;
            _db = db;
        }

        public LoginResponse Login(LoginRequest request)
        {
            var data = _db.Employees
                .Where(p => p.NIK.ToLower().Equals(request.NIK))
                .AsNoTracking()
                .FirstOrDefault();

            // Check account is not registered
            if (data == null) throw new Exception("Akun belum terdaftar!");

            // Verify the password
            var hasher = new Hasher();
            var verifiyPasswordUser = hasher.HashString(request.Password);
            if (!verifiyPasswordUser.Equals(data.Password)) throw new Exception("NIK atau Password yang anda masukkan salah!");

            // Check password expired 30 days
            var lastUpdatePassword = data.LastUpdatePassword;
            var dateNow = DateTime.Now;
            TimeSpan rangeDates = dateNow - lastUpdatePassword;
            int totalDays = (int)rangeDates.TotalDays + 1;

            if (totalDays >= 30) throw new Exception("Password anda sudah kadaluarsa, mohon untuk reset password!");

            string token = "";
            try
            {
                token = GenerateToken(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi kesalahan");
            }

            LoginResponse response = new LoginResponse();
            response.token = token;

            return response;
        }

        // To generate token login
        private string GenerateToken(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Name),
                new Claim(ClaimTypes.Role,user.Position),
                new Claim("uid", user.NIK)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
