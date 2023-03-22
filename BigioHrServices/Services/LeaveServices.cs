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
        public DatatableResponse GetList(LeaveSearchRequest request);
        void AddNewLeaveRequest(AddNewLeaveRequest request);
    }

    public class LeaveService : ILeaveService
	{
        private readonly ApplicationDbContext _db;
        private readonly int maxLeave = 12;
        private readonly int maxLeaveForHighestPosition = 2;
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;

        public LeaveService(ApplicationDbContext db, IEmployeeService employeeService, IPositionService positionService)
		{
			_db = db;
            _employeeService = employeeService;
            _positionService = positionService;

        }

        public void Approve(int id)
        {
            // todo logic melimpahkan
            var leave = _getLeaveDataOrFail(id);

            if (leave.IsAlreadyReviewed())
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
            // validasi matrix pelimpahan
            var delegationList = _db.Delegations
                .Where(x => x.ParentNIK == request.EmployeeNik)
                .ToList();
            if (delegationList.Count < 1)
            {
                throw new Exception("Employee don't have delegation list");
            }
            if (delegationList.Find(x => x.NIK == request.DelegatedNiK) == null)
            {
                throw new Exception("Delegated employee not exist in delegation list");
            }
            
            // get reviewer
            var reviewer = _getReviewerForNik(request.EmployeeNik);

            var isRequestingUserHaveHighestPosition = reviewer == null;

            if (isRequestingUserHaveHighestPosition)
            {
                if (_getLeaveQuotaForHighestPosition(request.EmployeeNik) < 1)
                {
                    throw new Exception("Insufficient leave quota");
                }
            }
            else
            {
                var quotaResponse = GetLeaveQuota(request.EmployeeNik);
                if (quotaResponse.LeaveAvailable < 1)
                {
                    throw new Exception("Insufficient leave quota");
                }
            }

            var leaveData = new Leave
            {
                StafNIK = request.EmployeeNik,
                DelegatedStafNIK = request.DelegatedNiK,
                ReviewerNIK = isRequestingUserHaveHighestPosition ? "SYSTEM" : reviewer.NIK,
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

            if (isRequestingUserHaveHighestPosition)
            {
                Approve(leaveData.Id);
            }
        }
        private int _getLeaveQuotaForHighestPosition(string requestEmployeeNik)
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            DateOnly firstDay = new DateOnly(currentYear, currentMonth, 1);
            DateOnly lastDay = new DateOnly(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));

            //get employee quota, base on current year 
            var nUsedLeave = _db.Leaves
                .Where(leave => leave.StafNIK == requestEmployeeNik)
                .Where(leave => leave.Status == Db.Entities.Leave.RequestStatus.Approved)
                .Where(leave => leave.LeaveDate >= firstDay && leave.LeaveDate <= lastDay)
                .ToList()
                .Count();
            return nUsedLeave - maxLeaveForHighestPosition;
        }
        private Employee? _getReviewerForNik(string nik)
        {
            var requestingUser = _employeeService.GetEmployeeByNIK(nik);
            if (requestingUser == null)
            {
                throw new Exception("request employee data not found");
            }
            var position = _positionService.GetPositionByCode(requestingUser.PositionCode.ToLower());
            if (position == null)
            {
                throw new Exception("Unexpected error");
            }
            var iPositionLevel = Convert.ToInt32(position.Level);
            // get higher position (have lower number)
            var reviewerPosition = _db.Positions
                .AsNoTracking()
                .Where(x => Convert.ToInt32(x.Level) < iPositionLevel)
                .OrderByDescending(x => Convert.ToInt32(x.Level))
                .FirstOrDefault();
            if (reviewerPosition == null)
            {
                // position is highest
                return null;
            }
            var reviewer = _db.Employees
                .AsNoTracking()
                .FirstOrDefault(x => x.PositionCode == reviewerPosition.Code);
            if (reviewer == null)
            {
                throw new Exception("Unexpected error");
            }
            return reviewer;
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
                case "leavedate":
                    query = query.OrderBy(leave => leave.LeaveDate);
                    break;
                case "leavestart_desc":
                    query = query.OrderByDescending(leave => leave.LeaveDate);
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
                    LeaveDate = _leave.LeaveDate,
                    CreatedAt = _leave.CreatedAt,
                    UpdatedAt = _leave.UpdatedAt,
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

