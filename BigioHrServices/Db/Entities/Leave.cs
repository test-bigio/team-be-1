using System;
namespace BigioHrServices.Db.Entities
{
	public class Leave
	{
        public enum RequestStatus
        {
            Approved,
			Rejected,
			Expired
        }

        public int Id { get; set; }
		public string StafNIK { get; set; } = string.Empty;
		public string DelegatedStafNIK { get; set; } = string.Empty;
		public string ReviewerNIK { get; set; } = string.Empty;
		public RequestStatus Status { get; set; }
		public DateTime LeaveStart { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}

