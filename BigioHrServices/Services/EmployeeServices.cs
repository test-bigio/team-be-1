using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace BigioHrServices.Services
{
    public interface IEmployeeService
    {
        public Pageable<EmployeeResponse> GetList(EmployeeSearchRequest request);
        public EmployeeResponse GetEmployeeByNIK(string nik);
        public SingleReponse GetDetail(string nik);
        public void EmployeeAdd(EmployeeAddRequest request);
        public void EmployeeUpdate(EmployeeUpdateRequest request);
        public void EmployeeDelete(string nik);
    }
    public class EmployeeServices : IEmployeeService
    {
        private readonly ApplicationDbContext _db;
        public EmployeeServices(ApplicationDbContext db)
        {
            _db = db;
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

            var pagedData = new Pageable<EmployeeResponse>(data, request.Page, request.PageSize);
            foreach (var response in pagedData.Content.Where(p => p.PositionCode != null))
            {
                response.Position = _db.Positions
                    .Where(p => p.Code == response.PositionCode)
                    .FirstOrDefault()?
                    .Name;
            }

            return pagedData;
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
                    JoinDate = _employee.JoinDate.ToString("yyy-MM-dd"),
                    WorkLength = _employee.WorkLength,
                    PositionCode = _employee.PositionCode,
                    IsActive = _employee.IsActive,
                    DigitalSignature = _employee.DigitalSignature,
                })
                .FirstOrDefault();
            /*if (data == null) throw new Exception("NIK tidak ada!");
            return data;*/
        }

        public SingleReponse GetDetail(string nik)
        {

            var query = _db.Employees
                .Where(p => p.NIK == nik)
                .AsNoTracking()
                .AsQueryable();

            var data = query
                .Select(_employee => new EmployeeResponse
                {
                    NIK = _employee.NIK,
                    Name = _employee.Name,
                    Sex = _employee.Sex,
                    //JoinDate = _employee.JoinDate,
                    WorkLength = _employee.WorkLength,
                    PositionCode = _employee.PositionCode,
                    IsActive = _employee.IsActive,
                    DigitalSignature = _employee.DigitalSignature,
                })
                .ToList();

            return new SingleReponse()
            {
                Data = data.ToArray()
            };
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
                    PositionID = request.PositionID,
                    Password = "Pegawai",
                    DigitalSignature = "101010",
                    IsActive = true
                }); ;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EmployeeUpdate(EmployeeUpdateRequest request)
        {
            //if (data != null) throw new Exception("NIK sudah ada!");
            var data = _db.Employees.SingleOrDefault(p => p.NIK == request.NIK);
            if (data != null)
            {
                try
                {
                    data.Name = request.Name;
                    data.Sex = request.Sex;
                    data.JoinDate = DateOnly.ParseExact(request.JoinDate, "yyyy-MM-dd");
                    data.WorkLength = request.WorkLength;
                    data.PositionID = request.PositionId;
                    data.UpdatedBy = null;
                    data.UpdatedDate = DateTime.UtcNow;
                    data.IsOnLeave = request.IsOnLeave;
                    data.Email = request.Email;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("NIK tidak ditemukan");
            }

        }

        public void EmployeeDelete(string nik)
        {
            var data = _db.Employees.SingleOrDefault(p => p.NIK == nik);
            if (data == null) throw new Exception("NIK Tidak Ditemukan!");

            try
            {
                data.IsActive = false;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
