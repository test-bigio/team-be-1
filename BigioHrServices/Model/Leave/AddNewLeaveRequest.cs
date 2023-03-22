namespace BigioHrServices.Model.Leave
{
    public class AddNewLeaveRequest
    {
        public string EmployeeNik { get; set; } = string.Empty;
        public string DelegatedNiK { get; set; } = string.Empty;
        public DateTime LeaveDate { get; set; }
    }
}
