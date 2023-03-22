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

namespace BigioHrServices.Services
{
    public interface IPositionService
    {
        public DatatableResponse GetListPosition(PositionSearchRequest request);
        public PositionResponse GetPositionByCode(string code);
        public Position GetDetailPosition(string code);
        public void AddPosition(PositionRequest request);
        public void EditPosition(PositionRequest request);
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

        public PositionResponse GetPositionByCode(string code)
        {
            return _db.Positions
                .Where(p => p.Code.ToLower() == code)
                .AsNoTracking()
                .Select(_position => new PositionResponse
                {
                    Code = _position.Code,
                    Name = _position.Name,
                    Level = _position.Level,
                    IsActive = _position.IsActive,
                })
                .FirstOrDefault();
        }

        public void AddPosition(PositionRequest request)
        {
            var data = _db.Positions
                .Where(p => p.Code.ToLower() == request.Code)
                .AsNoTracking()
                .FirstOrDefault();

            if (data != null) throw new Exception("Code is Exist!");
            try
            {
                _db.Positions.Add(new Position
                {
                    Code = request.Code,
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

        public void EditPosition(PositionRequest request)
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
