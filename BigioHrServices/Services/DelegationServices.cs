using System;
using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Delegation;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace BigioHrServices.Services
{
    public interface IDelegationService
    {
        public DatatableResponse GetList(DelegationSearchRequest request);
        public void DelegationAdd(DelegationAddRequest request);
    }

    public class DelegationServices : IDelegationService
	{
        private readonly ApplicationDbContext _db;

        public DelegationServices(ApplicationDbContext db)
		{
			_db = db;
		}

        public DatatableResponse GetList(DelegationSearchRequest request)
        {

            var query = _db.Delegations
                .AsNoTracking()
                .AsQueryable()
                .AsEnumerable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.ParentNIK == request.Search);
            }

            var data = query
                .Select(_delegation => new DelegationResponse
                {
                    NIK = _delegation.NIK,
                    ParentNIK = _delegation.ParentNIK,
                    Priority = _delegation.Priority
                })
                .GroupBy(x => new {x.ParentNIK})
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

        public void DelegationAdd(DelegationAddRequest request)
        {
            var data = _db.Delegations
                .Where(p => p.ParentNIK == request.ParentNIK)
                .AsNoTracking()
                .ToList();
            if (data.Count >= 3) throw new Exception("NIK "+request.ParentNIK+"sudah memiliki 3 backup");

            try
            {
                _db.Delegations.Add(new Delegation
                {
                    NIK = request.NIK,
                    ParentNIK = request.ParentNIK,
                    Priority = request.Priority
                }); ;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

