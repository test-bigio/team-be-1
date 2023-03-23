using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BigioHrServices.Services
{
    public interface IEmployeeService
    {
        public Pageable<EmployeeResponse> GetList(EmployeeSearchRequest request);
        public EmployeeResponse GetDetailEmployees(string nik);
        public Employee GetEmployeeByNIK(string nik);
        public void EmployeeAdd(EmployeeAddRequest request);
        public void EmployeeUpdate(EmployeeUpdateRequest request);
        public void EmployeeDelete(string nik);

    }
    public class EmployeeServices : IEmployeeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuditModuleServices _auditLogService;
        public EmployeeServices(ApplicationDbContext db, IAuditModuleServices auditLogService)
        {
            _db = db;
            _auditLogService = auditLogService;
        }

        public Pageable<EmployeeResponse> GetList(EmployeeSearchRequest request)
        {

            var query = _db.Employees
                .Where(p => p.IsActive == request.IsActive)
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.NIK.ToLower() == request.Search.ToLower() || p.Name.ToLower() == request.Search.ToLower());
            }
            if (!string.IsNullOrEmpty(request.JoinDateRangeBegin))
            {
                query = query.Where(p => p.JoinDate > DateOnly.ParseExact(request.JoinDateRangeBegin, "yyyy-MM-dd"));
            }

            if (!string.IsNullOrEmpty(request.JoinDateRangeEnd))
            {
                query = query.Where(p => p.JoinDate < DateOnly.ParseExact(request.JoinDateRangeEnd, "yyy-MM-dd"));
            }


            var data = query
                .Select(_employee => new EmployeeResponse
                {
                    NIK = _employee.NIK,
                    Name = _employee.Name,
                    Sex = _employee.Sex,
                    JoinDate = _employee.JoinDate.ToString("yyy-MM-dd"),
                    WorkLength = _employee.WorkLength,
                    PositionCode = _employee.PositionCode,
                    IsActive = _employee.IsActive,
                    DigitalSignature = _employee.DigitalSignature,
                })
                .ToList();

            var pagedData = new Pageable<EmployeeResponse>(data, request.Page - 1, request.PageSize);
            foreach (var response in pagedData.Content.Where(p => p.PositionCode != null))
            {
                response.Position = _db.Positions
                    .Where(p => p.Code == response.PositionCode)
                    .FirstOrDefault()?
                    .Name;
            }

            _auditLogService.CreateLog(
                           "Pegawai",
                           "List",
                           "Get List"
                       );

            return pagedData;
        }

        public EmployeeResponse GetDetailEmployees(string nik)
        {
            var data = _db.Employees
                .Select(_employee => new EmployeeResponse
                {
                    NIK = _employee.NIK,
                    Name = _employee.Name,
                    Sex = _employee.Sex,
                    JoinDate = _employee.JoinDate.ToString("yyy-MM-dd"),
                    WorkLength = _employee.WorkLength,
                    PositionCode = _employee.PositionCode,
                    IsActive = _employee.IsActive,
                    DigitalSignature = _employee.DigitalSignature,
                })
                .FirstOrDefault(p => p.NIK == nik);

            _auditLogService.CreateLog(
                      "Pegawai",
                      "Detail Pegawai",
                      "Get Detail Pegawai"
                  );
            return data;
        }

        public Employee GetEmployeeByNIK(string nik)
        {
            return _db.Employees
                .Where(p => p.NIK.ToLower() == nik)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void EmployeeAdd(EmployeeAddRequest request)
        {
            var data = GetEmployeeByNIK(request.NIK);
            if (data != null) throw new Exception("NIK sudah ada!");

            string hashPassword = Hasher.HashString2("Pegawai");
            string hashDigitalPin = Hasher.HashString2("101010");

            try
            {
                var newEmployee = new Employee
                {
                    NIK = request.NIK,
                    Name = request.Name,
                    Sex = request.Sex,
                    JoinDate = DateOnly.ParseExact(request.JoinDate, "yyyy-MM-dd"),
                    WorkLength = request.WorkLength,
                    PositionCode = request.PositionID,
                    Password = hashPassword,
                    DigitalSignature = hashDigitalPin,
                    IsActive = true,
                    Email = request.Email
                };

                _db.Employees.Add(newEmployee);
                _db.SaveChanges();

                _auditLogService.CreateLog(
                          "Pegawai",
                          "Add",
                          "Add Pegawai"
                      );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EmployeeUpdate(EmployeeUpdateRequest request)
        {
            var data = _db.Employees.SingleOrDefault(p => p.NIK == request.NIK);
            if (data != null)
            {
                try
                {
                    data.Name = request.Name;
                    data.Sex = request.Sex;
                    data.JoinDate = DateOnly.ParseExact(request.JoinDate, "yyyy-MM-dd");
                    data.WorkLength = request.WorkLength;
                    data.PositionCode = request.PositionId;
                    data.UpdatedBy = null;
                    data.UpdatedDate = DateTime.UtcNow;
                    data.IsOnLeave = request.IsOnLeave;
                    data.Email = request.Email;
                    _db.SaveChanges();

                    _auditLogService.CreateLog(
                          "Pegawai",
                          "Update",
                          "Sukses Update"
                      );
                }
                catch (Exception ex)
                {
                    _auditLogService.CreateLog(
                         "Pegawai",
                          "Update",
                          "Gagal Update"
                      );
                    throw ex;
                }
            }
            else
            {
                _auditLogService.CreateLog(
                         "Pegawai",
                          "Update",
                          "NIK tidak ditemukan"
                      );
                throw new Exception("NIK tidak ditemukan");
            }

        }

        public void EmployeeDelete(string nik)
        {
            var data = _db.Employees.SingleOrDefault(p => p.NIK == nik);

            try
            {
                data.IsActive = false;
                _db.SaveChanges();

                _auditLogService.CreateLog(
                        "Pegawai",
                         "Non Active",
                         "Sukses Non Active"
                     );
            }
            catch (Exception ex)
            {
                _auditLogService.CreateLog(
                        "Pegawai",
                         "Non Active",
                         "Gagal Non Active"
                     );
                throw ex;
            }


        }
    }
}
