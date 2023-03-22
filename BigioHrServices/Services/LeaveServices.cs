using System;
using BigioHrServices.Db;
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
            throw new NotImplementedException();
        }

        public void Reject(int id)
        {
            throw new NotImplementedException();
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
    }
}

