using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Position;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace BigioHrServices.Services
{
    public interface IPositionService
    {
        public DatatableResponse GetListPosition(PositionSearchRequest request);
        public Position GetPositionByCode(string code);
        public Position GetDetailPosition(string code);
        public void AddPosition(PositionAddRequest request);
        public void EditPosition(PositionEditRequest request);
        public void InactivePosition(string code);

    }
    public class PositionServices : IPositionService
    {
        private readonly ApplicationDbContext _db;
        public PositionServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public DatatableResponse GetListPosition(PositionSearchRequest request)
        {

            var query = _db.Positions
                .Where(p => p.IsActive == request.IsActive)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.Code.ToLower() == request.Search.ToLower() || p.Name.ToLower() == request.Search.ToLower());
            }

            var data = query
                .Select(_position => new PositionResponse
                {
                    Code = _position.Code,
                    Name = _position.Name,
                    Level = _position.Level,
                    IsActive = _position.IsActive,
                })
                .ToList();

            return new DatatableResponse()
            {
                Data = data.ToArray(),
                TotalRecords = data.Count,
                PageSize = request.PageSize > data.Count ? data.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < data.Count,
                PrevPage = request.Page > 1,
            };
        }

        public Position GetDetailPosition(string code)
        {
            return _db.Positions.Find(code);
        }

        public Position GetPositionByCode(string code)
        {
            return _db.Positions
                .Where(p => p.Code.ToLower() == code.ToLower())
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void AddPosition(PositionAddRequest request)
        {
            var chars = "0123456789";
            var array = new char[4];
            var random = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = chars[random.Next(chars.Length)];
            }

            var transaksiId = new String(array);

            string prefix = "Pos";
            string fourDigit = transaksiId.ToString().PadLeft(4, '0');
            string uniqueCode = prefix + fourDigit;

            try
            {
                _db.Positions.Add(new Position
                {

                    Code = uniqueCode,
                    Name = request.Name,
                    Level = request.Level,
                    IsActive = true
                });
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EditPosition(PositionEditRequest request)
        {
            var data = _db.Positions.SingleOrDefault(p => p.Code == request.Code);
            if (data != null)
            {
                try
                {
                    data.Name = request.Name;
                    data.Level = request.Level;

                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                throw new Exception("Code is not Exist!");
            }

        }

        public void InactivePosition(string code)
        {
            var data = _db.Positions.SingleOrDefault(p => p.Code == code);
            if (data != null)
            {
                try
                {
                    data.IsActive = false;

                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
