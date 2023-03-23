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
        public DelegationDetailResponse GetDetailDelegation(string nik);
        public void DelegationAdd(DelegationAddRequest request);
    }

    public class DelegationServices : IDelegationService
	{
        private readonly ApplicationDbContext _db;

        private readonly IAuditModuleServices _auditLogService;

        public DelegationServices(ApplicationDbContext db, IAuditModuleServices auditLogService)
		{
			_db = db;
            _auditLogService = auditLogService;
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
                .GroupBy(x => new {x.ParentNIK})
                .Join(_db.Employees, d => d.Key.ParentNIK, 
                    e => e.NIK, (d, e) => new 
                    {
                        NIK = e.NIK,
                        Name = e.Name,
                        Count = d.Count()
                    })
                .Select(g => new {
                    NIK = g.NIK,
                    Name = g.Name,
                    BackupCount = g.Count
                })
                .ToList();

            _auditLogService.CreateLog(
                "Delegation",
                "List",
                "Get List"
            );

            return new DatatableResponse()
            {
                Data = data.ToArray(),
                TotalRecords = data.Count,
                PageSize = request.PageSize > data.Count ? data.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < data.Count,
                PrevPage = request.Page > 1,
            };
        }

        public DelegationDetailResponse GetDetailDelegation(string nik)
        {
            var data = _db.Delegations
                .Where(p => p.ParentNIK == nik)
                .Select(_delegation => new DelegationResponse
                {
                    NIK = _delegation.NIK,
                    ParentNIK = _delegation.ParentNIK,
                    Priority = _delegation.Priority
                })
                .OrderBy(x => x.Priority)
                .ToList();
            
            if (data.Count == 0) {
                _auditLogService.CreateLog(
                    "Delegation",
                    "Detail Delegation",
                    "NIK tidak ditemukan"
                );
                throw new Exception("NIK tidak ditemukan");
            }

            _auditLogService.CreateLog(
                "Delegation",
                "Detail Delegation",
                "Get Detail Delegation"
            );

            return new DelegationDetailResponse()
            {
                NIK = nik,
                Backups = data.ToArray()
            };
        }

        public void DelegationAdd(DelegationAddRequest request)
        {
            var dataEmployee = _db.Employees
                .Where(p => p.NIK == request.ParentNIK)
                .FirstOrDefault();
            
            if (dataEmployee == null) {
                _auditLogService.CreateLog(
                    "Delegation",
                    "Add",
                    "NIK Pegawai "+request.ParentNIK+" tidak ditemukan."
                );
                throw new Exception("NIK Pegawai "+request.ParentNIK+" tidak ditemukan.");
            }

            var dataEmployeeBackup = _db.Employees
                .Where(p => p.NIK == request.NIK)
                .FirstOrDefault();

            if (dataEmployeeBackup == null) {
                _auditLogService.CreateLog(
                    "Delegation",
                    "Add",
                    "NIK Backup "+request.NIK+" tidak ditemukan."
                );
                throw new Exception("NIK Backup "+request.NIK+" tidak ditemukan.");
            }

            var data = _db.Delegations
                .Where(p => p.ParentNIK == request.ParentNIK)
                .AsNoTracking()
                .ToList();

            if (data.Count >= 3) {
                _auditLogService.CreateLog(
                    "Delegation",
                    "Add",
                    "NIK "+request.ParentNIK+" sudah memiliki 3 backup."
                );
                throw new Exception("NIK "+request.ParentNIK+" sudah memiliki 3 backup.");
            }

            var dataNIKBackup = _db.Delegations
                .Where(p => p.NIK == request.NIK)
                .AsNoTracking()
                .ToList();

            if (dataNIKBackup.Count >= 2) {
                _auditLogService.CreateLog(
                    "Delegation",
                    "Add",
                    "NIK "+request.NIK+" sudah jadi 2 backup pegawai."
                );
                throw new Exception("NIK "+request.NIK+" sudah jadi 2 backup pegawai.");
            }

            try
            {
                _db.Delegations.Add(new Delegation
                {
                    NIK = request.NIK,
                    ParentNIK = request.ParentNIK,
                    Priority = request.Priority
                });
                _db.SaveChanges();

                _auditLogService.CreateLog(
                    "Delegation",
                    "Add",
                    "Add Delegation"
                );
            }
            catch (Exception ex)
            {
                _auditLogService.CreateLog(
                    "Delegation",
                    "Add",
                    "Gagal Add Delegation"
                );
                throw ex;
            }
        }

    }
}

