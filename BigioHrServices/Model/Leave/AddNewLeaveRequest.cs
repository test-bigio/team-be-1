namespace BigioHrServices.Model.Leave
{
    public class AddNewLeaveRequest
    {
        public string EmployeeNIk { get; set; } = string.Empty;
        public string DelegatedNIK { get; set; } = string.Empty;
        public DateTime LeaveDate { get; set; }
    }
}
