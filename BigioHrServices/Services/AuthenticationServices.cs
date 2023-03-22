using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
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
        public BaseResponse ResetPassword(ResetPasswordRequest request);
        public BaseResponse AddPinSignature(AddPinSignatureRequest request);
        public BaseResponse UpdatePinSignature(UpdatePinSignatureRequest request);
    }
    public class AuthenticationServices : IAuthenticationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly Hasher _hasher;

        public AuthenticationServices(IConfiguration config, ApplicationDbContext db, Hasher hasher)
        {
            _config = config;
            _db = db;
            _hasher = hasher;
        }


        #region MethodLogin
        public LoginResponse Login(LoginRequest request)
        {
            var data = _db.Employees
                .Where(p => p.NIK.ToLower().Equals(request.NIK))
                .AsNoTracking()
                .FirstOrDefault();

            // Check account is not registered
            if (data == null) throw new Exception("Akun belum terdaftar!");

            // Verify the password
            if (!_hasher.verifiyPassword(request.Password, data.Password)) throw new Exception("NIK atau Password yang anda masukkan salah!");

            // Check password expired 30 days
            var lastUpdatePassword = data.LastUpdatePassword;
            var dateNow = DateTime.Now;
            TimeSpan rangeDates = dateNow - lastUpdatePassword;
            int totalDays = (int)rangeDates.TotalDays + 1;          
            if (totalDays >= 30) throw new Exception("Password anda sudah kadaluarsa, mohon untuk reset password!");

            string token = "";
            
            // TODO: If pass validation generate token login
            try
            {
                token = GenerateToken(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            LoginResponse response = new LoginResponse();
            response.token = token;

            return response;
        }
        #endregion


        #region GenerateTokenLogin
        private string GenerateToken(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Name),
                new Claim(ClaimTypes.Role,user.PositionCode),
                new Claim("uid", user.NIK),
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
        #endregion


        #region MethodResetPassword
        public BaseResponse ResetPassword(ResetPasswordRequest request)
        {
            // Get data employee by nik
            var data = _db.Employees
                        .Where(x => x.NIK.ToLower().Equals(request.NIK))
                        .AsNoTracking()
                        .FirstOrDefault();

            // Check if data employee not found
            if (data == null) throw new Exception("Akun tidak ditemukan!");

            // Check current password not same with old password
            if (!_hasher.verifiyPassword(request.CurrentPassword, data.Password)) throw new Exception("Password sekarang tidak sesuai!");

            // Check if new password is same with current password
            if (_hasher.verifiyPassword(request.NewPassword, data.Password)) throw new Exception("Password baru tidak boleh sama dengan password sekarang!");

            // TODO: If pass validation save new password into table
            try 
            {
                data.Password = _hasher.HashString(request.NewPassword);
                data.LastUpdatePassword = DateTime.Now;

                _db.Employees.Update(data);
                _db.SaveChanges();

                return new BaseResponse
                {
                    Message = "Reset password berhasil, silahkan login kembali"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        
        #region MethodAddPinSignature
        public BaseResponse AddPinSignature(AddPinSignatureRequest request)
        {
            // Hash new pin signature
            var digitalSignatureHashed = _hasher.HashString(request.newPinSignature);

            // Get data employee by nik
            var data = _db.Employees
                        .Where(x => x.NIK.ToLower().Equals(request.NIK))
                        .AsNoTracking()
                        .FirstOrDefault();
            
            // Check if data employee not found
            if (data == null) throw new Exception("Akun tidak ditemukan!");

            try
            {
                // TODO: If pass validation save pin signature to tabel
                data.DigitalSignature = digitalSignatureHashed;
                _db.Employees.Update(data);
                _db.DigitalPinLogs.Add(new DigitalPinLog
                {
                    StaffId = data.NIK,
                    Pin = digitalSignatureHashed,
                    CreatedAt = DateTime.Now
                });

                _db.SaveChanges();

                return new BaseResponse
                {
                    Message = "Digital signature telah ditambahkan!"
                };
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region MethodUpdatePinSignature
        public BaseResponse UpdatePinSignature(UpdatePinSignatureRequest request)
        {
            // Hash new pin signature
            var digitalSignatureHashed = _hasher.HashString(request.newPinSignature);
            
            // Get data employee by nik
            var data = _db.Employees
                        .Where(x => x.NIK.ToLower().Equals(request.NIK))
                        .AsNoTracking()
                        .FirstOrDefault();
            
            var historyPin = _db.DigitalPinLogs
                            .Where(x => (x.StaffId.ToLower().Equals(request.NIK)) && (x.Pin.Equals(digitalSignatureHashed)))
                            .AsNoTracking()
                            .OrderByDescending(x => x.CreatedAt)
                            .FirstOrDefault();
            
            // Check if data employee not found
            if (data == null) throw new Exception("Akun tidak ditemukan!");

            // Check current signature
            if (!_hasher.verivyPIN(request.oldPinSignature, data.DigitalSignature)) throw new Exception("PIN sekarang tidak sesuai!"); 

            // Check new signature must not equal with current signature
            if (_hasher.verivyPIN(request.newPinSignature, data.DigitalSignature)) throw new Exception("PIN baru tidak boleh sama dengan PIN sekarang");

            // Check if signatures have been used
            if (historyPin != null)
            {
                if (_hasher.verivyPIN(request.newPinSignature, historyPin.Pin)) throw new Exception("PIN pernah digunakan sebelumnya!");
            }

            try
            {
                // TODO: If pass validation save pin signature to tabel
                data.DigitalSignature = digitalSignatureHashed;
                _db.Employees.Update(data);
                _db.DigitalPinLogs.Add(new DigitalPinLog
                {
                    StaffId = data.NIK,
                    Pin = digitalSignatureHashed,
                    CreatedAt = DateTime.Now
                });

                _db.SaveChanges();

                return new BaseResponse
                {
                    Message = "Digital signature berhasil diupdate!"
                };
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
