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
        void AddNewLeaveRequest(AddNewLeaveRequest request);
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

            // add to notification
            var notification = new Notification
            {
                Nik = leave.StafNIK,
                Title = "Status Pengajuan Cuti",
                Body = "Disetujui",
                CreatedDate = DateTime.UtcNow,
            };
            _db.Notifications.Add(notification);
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

            // add to notification
            var notification = new Notification
            {
                Nik = leave.StafNIK,
                Title = "Status Pengajuan Cuti",
                Body = "Ditolak",
                CreatedDate = DateTime.UtcNow,
            };
            _db.Notifications.Add(notification);
            _db.SaveChanges();
        }

        public LeaveQuotaResponse GetLeaveQuota(string nik)
        {
            int currentYear = DateTime.Now.Year;
            DateOnly firstDay = new DateOnly(currentYear, 1, 1);
            DateOnly lastDay = new DateOnly(currentYear, 12, 31);

            var employee = _db.Employees.SingleOrDefault(employee => employee.NIK == nik);
            if (employee == null) throw new Exception("Employee not found");
            
            //get employee quota, base on current year 
            var totalLeave = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveDate >= firstDay && leave.LeaveDate <= lastDay)
                .ToList()
                .Count();

            return new LeaveQuotaResponse
            {
                LeaveAvailable = maxLeave - totalLeave
            };
        }

        public DatatableResponse GetLeaveHistory(string nik, LeaveHistoryRequest request)
        {
            int currentYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(request.Search)) currentYear = Int16.Parse(request.Search);
            DateOnly firstDay = new DateOnly(currentYear, 1, 1);
            DateOnly lastDay = new DateOnly(currentYear, 12, 31);
            
            var employee = _db.Employees.SingleOrDefault(employee => employee.NIK == nik);
            if (employee == null) throw new Exception("Employee not found");
            var totalRecord = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveDate >= firstDay && leave.LeaveDate <= lastDay)
                .Select(leave => leave.StafNIK)
                .Count();
            
            var leaves = _db.Leaves
                .Where(leave => leave.StafNIK == nik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveDate >= firstDay && leave.LeaveDate <= lastDay)
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
        public void AddNewLeaveRequest(AddNewLeaveRequest request)
        {
            // todo validasi matrik pelimpahan
            
            // todo get this flag from position
            var currentUserHaveHighestPosition = false;

            if (currentUserHaveHighestPosition)
            {
                // todo if user have highest position then max quota permonth = 2
            }
            else
            {
                var quotaResponse = GetLeaveQuota(request.EmployeeNIk);
                if (quotaResponse.LeaveAvailable < 1)
                {
                    throw new Exception("Insufficient leave quota");
                }
            }

            var reviewerNik = "SYSTEM";
            if (!currentUserHaveHighestPosition)
            {
                var reviewer = _getReviewerForNik(request.EmployeeNIk);
                reviewerNik = reviewer.NIK;
            }
            var leaveData = new Leave
            {
                StafNIK = request.EmployeeNIk,
                DelegatedStafNIK = request.DelegatedNIK,
                ReviewerNIK = reviewerNik,
                Status = Leave.RequestStatus.InReview,
                LeaveDate = DateOnly.FromDateTime(request.LeaveDate),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _db.Leaves.Add(leaveData);
            _db.SaveChanges();

            // add to notification
            var notification = new Notification
            {
                Nik = leaveData.StafNIK,
                Title = "Pengajuan Cuti Baru",
                Body = "Pengajuan cuti dari pegawai dengan nik "+ leaveData.StafNIK + "silahkan di review",
                CreatedDate = DateTime.UtcNow,
            };
            _db.Notifications.Add(notification);
            _db.SaveChanges();

            if (currentUserHaveHighestPosition)
            {
                // todo: still not test this implementation
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

