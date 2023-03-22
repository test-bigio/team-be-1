namespace BigioHrServices.Model.Leave
{
    public class AddNewLeaveRequest
    {
        public string DelegatedNIK { get; set; } = string.Empty;
        public DateTime LeaveStart { get; set; }
        public int TotalLeaveInDays { get; set; }
    }
}
