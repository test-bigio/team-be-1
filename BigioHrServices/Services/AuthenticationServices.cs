using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Authentication;
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
                .Where(p => p.NIK.ToLower().Equals(request.NIK) && p.Password.Equals(request.Password))
                .AsNoTracking()
                .FirstOrDefault();
            
            if (data == null) throw new Exception("NIK atau password tidak sesuai!");

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



        // To generate token
        private string GenerateToken(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Name),
                new Claim(ClaimTypes.Role,user.Position)
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
