using System;
using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Leave;

namespace BigioHrServices.Services
{
    public interface ILeaveService
    {
        void Approve(int id);
        void Reject(int id);
        LeaveQuotaResponse GetLeaveQuota(string id);
        DatatableResponse GetLeaveHistory(string id, LeaveHistoryRequest request);
        void AddNewLeaveRequest(AddNewLeaveRequest request, string currentUserNik);
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
            int currentYear = DateTime.Now.Year;
            DateTime firstDay = new DateTime(currentYear, 1, 1);
            DateTime lastDay = new DateTime(currentYear, 12, 31);

            var employee = _db.Employees.SingleOrDefault(employee => employee.NIK == nik);
            if (employee == null) throw new Exception("Employee not found");
            
            //get employee quota, base on current year 
            var leaveCount = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveStart.Date >= firstDay && leave.LeaveStart.Date <= lastDay)
                .ToList()
                .Count();

            return new LeaveQuotaResponse
            {
                LeaveAvailable = maxLeave - leaveCount
            };
        }

        public DatatableResponse GetLeaveHistory(string nik, LeaveHistoryRequest request)
        {
            var employee = _db.Employees.SingleOrDefault(employee => employee.NIK == nik);
            if (employee == null) throw new Exception("Employee not found");
            
            var leaves = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .ToList();

            Console.WriteLine(leaves);
            return new DatatableResponse()
            {
                Data = leaves.ToArray(),
                TotalRecords = leaves.Count,
                PageSize = request.PageSize > leaves.Count ? leaves.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < leaves.Count,
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
    }
}

