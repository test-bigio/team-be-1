using System;
using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Leave;
using Microsoft.EntityFrameworkCore;

namespace BigioHrServices.Services
{
    public interface ILeaveService
    {
        void Approve(int id);
        void Reject(int id);
        LeaveQuotaResponse GetLeaveQuota(string id);
        DatatableResponse GetLeaveHistory(string id, LeaveHistoryRequest request);
        void AddNewLeaveRequest(AddNewLeaveRequest request, string currentUserNik);
        public DatatableResponse GetList(LeaveSearchRequest request);
    }

    public class LeaveService : ILeaveService
	{
        private readonly ApplicationDbContext _db;
        private readonly int maxLeave = 12;

        public LeaveService(ApplicationDbContext db)
		{
			_db = db;
		}

        public void Approve(int id)
        {
            // todo logic melimpahkan
            var leave = _getLeaveDataOrFail(id);

            if (!leave.IsAlreadyReviewed())
            {
                throw new Exception("Request already reviewed");
            }
            
            leave.Status = Leave.RequestStatus.Approved;
            _db.SaveChanges();
        }

        public void Reject(int id)
        {
            var leave = _getLeaveDataOrFail(id);

            if (!leave.IsAlreadyReviewed())
            {
                throw new Exception("Request already reviewed");
            }
            leave.Status = Leave.RequestStatus.Rejected;
            _db.SaveChanges();
        }

        public LeaveQuotaResponse GetLeaveQuota(string nik)
        {
            int totalLeave = 0;
            int currentYear = DateTime.Now.Year;
            DateTime firstDay = new DateTime(currentYear, 1, 1);
            DateTime lastDay = new DateTime(currentYear, 12, 31);

            var employee = _db.Employees.SingleOrDefault(employee => employee.NIK == nik);
            if (employee == null) throw new Exception("Employee not found");
            
            //get employee quota, base on current year 
            var leaves = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveStart.Date >= firstDay && leave.LeaveStart.Date <= lastDay)
                .ToList();

            foreach (var leave in leaves)
            {
                totalLeave += leave.TotalLeaveInDays;
            }

            return new LeaveQuotaResponse
            {
                LeaveAvailable = maxLeave - totalLeave
            };
        }

        public DatatableResponse GetLeaveHistory(string nik, LeaveHistoryRequest request)
        {
            int currentYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(request.Search)) currentYear = Int16.Parse(request.Search);
            DateTime firstDay = new DateTime(currentYear, 1, 1);
            DateTime lastDay = new DateTime(currentYear, 12, 31);
            
            var employee = _db.Employees.SingleOrDefault(employee => employee.NIK == nik);
            if (employee == null) throw new Exception("Employee not found");
            var totalRecord = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveStart.Date >= firstDay && leave.LeaveStart.Date <= lastDay)
                .Select(leave => leave.StafNIK)
                .Count();
            
            var leaves = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveStart.Date >= firstDay && leave.LeaveStart.Date <= lastDay)
                .Skip((request.Page - 1 )* request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new DatatableResponse()
            {
                Data = leaves.ToArray(),
                TotalRecords = totalRecord,
                PageSize = request.PageSize > totalRecord ? totalRecord : request.PageSize,
                NextPage = (request.PageSize * request.Page) < totalRecord,
                PrevPage = request.Page > 1,
            };
        }
        public void AddNewLeaveRequest(AddNewLeaveRequest request, string currentUserNik)
        {
            // todo get this flag from position
            var currentUserHaveHighestPosition = false;
            if (currentUserHaveHighestPosition)
            {
                // todo if user have highest position then max quota permonth = 2
            }
            else
            {
                var quotaResponse = GetLeaveQuota(currentUserNik);
                if (quotaResponse.LeaveAvailable < request.TotalLeaveInDays)
                {
                    throw new Exception("Insufficient leave quota");
                }
            }
            // todo validasi matrik pelimpahan
            
            var reviewer = _getReviewerForNik(currentUserNik);
            var leaveData = new Leave
            {
                StafNIK = currentUserNik,
                DelegatedStafNIK = request.DelegatedNIK,
                ReviewerNIK = reviewer.NIK,
                Status = Leave.RequestStatus.InReview,
                LeaveStart = request.LeaveStart,
                TotalLeaveInDays = request.TotalLeaveInDays,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _db.Leaves.Add(leaveData);
            _db.SaveChanges();
            if (currentUserHaveHighestPosition)
            {
                // still not test this implementation
                // Approve(leaveData.Id);
            }
        }
        private Employee _getReviewerForNik(string currentUserNik)
        {
            // todo implement get reviewer for nik
            return new Employee { NIK = "69123" };
        }

        private Leave _getLeaveDataOrFail(int id)
        {
            var leave = _db.Leaves.Find(id);
            if (leave == null)
            {
                throw new Exception("Data not found");
            }
            return leave;
        }

        public DatatableResponse GetList(LeaveSearchRequest request)
        {

            var query = _db.Leaves
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(leave => leave.StafNIK.ToLower() == request.Search.ToLower() || leave.ReviewerNIK.ToLower() == request.Search.ToLower() || leave.DelegatedStafNIK.ToLower() == request.Search.ToLower());
            }

            if (!string.IsNullOrEmpty(request.stafNIK))
            {
                query = query.Where(leave => leave.StafNIK.ToLower() == request.stafNIK.ToLower());
            }

            if (!string.IsNullOrEmpty(request.reviewerNIK))
            {
                query = query.Where(leave => leave.ReviewerNIK.ToLower() == request.reviewerNIK.ToLower());
            }

            switch (request.SortBy.ToString().ToLower())
            {
                case "createdat":
                    query = query.OrderBy(leave => leave.CreatedAt);
                    break;
                case "createdat_desc":
                    query = query.OrderByDescending(leave => leave.CreatedAt);
                    break;
                case "leavestart":
                    query = query.OrderBy(leave => leave.LeaveStart);
                    break;
                case "leavestart_desc":
                    query = query.OrderByDescending(leave => leave.LeaveStart);
                    break;
                default:
                    query = query.OrderBy(leave => leave.Id);
                    break;
            }

            var data = query
                .Select(_leave => new LeaveResponse
                {
                    Id = _leave.Id,
                    StafNIK = _leave.StafNIK,
                    DelegatedStafNIK = _leave.DelegatedStafNIK,
                    ReviewerNIK = _leave.ReviewerNIK,
                    Status = _leave.Status.ToString(),
                    LeaveStart = _leave.LeaveStart,
                    CreatedAt = _leave.CreatedAt,
                    UpdatedAt = _leave.UpdatedAt,
                    TotalLeaveInDays = _leave.TotalLeaveInDays
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
    }
}

