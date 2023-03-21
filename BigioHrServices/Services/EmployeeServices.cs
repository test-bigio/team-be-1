using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Reflection.Metadata;

namespace BigioHrServices.Services
{
    public interface IEmployeeService
    {
        public DatatableResponse GetList(EmployeeSearchRequest request);
        public EmployeeResponse GetEmployeeByNIK(string nik);
        public void EmployeeAdd(EmployeeAddRequest request);
    }
    public class EmployeeServices : IEmployeeService
    {
        private readonly ApplicationDbContext _db;
        public EmployeeServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public DatatableResponse GetList(EmployeeSearchRequest request)
        {
            var query = _db.Employees
                .Where(p => 
                    p.NIK.ToLower() == request.Search.ToLower() ||
                    p.Name.ToLower() == request.Search.ToLower())
                .Where(p => p.IsActive == request.IsActive)
                .AsNoTracking()
                .AsQueryable();

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
                    JoinDate = _employee.JoinDate,
                    WorkLength = _employee.WorkLength,
                    Position = _employee.Position,
                    IsActive = _employee.IsActive,
                    DigitalSignature = _employee.DigitalSignature,
                })
                .ToList();
            return new DatatableResponse()
            {
                Data = data.Skip(request.PageSize * request.Page).Take(request.PageSize),
                TotalRecords = data.Count,
                PageSize = request.PageSize > data.Count ? data.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < data.Count,
                PrevPage = request.Page > 1,
            };
        }

        public EmployeeResponse GetEmployeeByNIK(string nik)
        {
            return _db.Employees
                .Where(p => p.NIK.ToLower() == nik)
                .AsNoTracking()
                .Select(_employee => new EmployeeResponse
                {
                    NIK = _employee.NIK,
                    Name = _employee.Name,
                    Sex = _employee.Sex,
                    JoinDate = _employee.JoinDate,
                    WorkLength = _employee.WorkLength,
                    Position = _employee.Position,
                    IsActive = _employee.IsActive,
                    DigitalSignature = _employee.DigitalSignature,
                })
                .FirstOrDefault();

            /*if (data == null) throw new Exception("NIK tidak ada!");
            return data;*/
        }

        public void EmployeeAdd(EmployeeAddRequest request)
        {
            var data = _db.Employees
                .Where(p => p.NIK.ToLower() == request.NIK)
                .AsNoTracking()
                .FirstOrDefault();
            if (data != null) throw new Exception("NIK sudah ada!");

            try
            {
                _db.Employees.Add(new Employee
                {
                    NIK = request.NIK,
                    Name = request.Name,
                    Sex = request.Sex,
                    JoinDate = DateOnly.ParseExact(request.JoinDate, "yyyy-MM-dd"),
                    WorkLength = request.WorkLength,
                    Position = request.Position
                });
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
