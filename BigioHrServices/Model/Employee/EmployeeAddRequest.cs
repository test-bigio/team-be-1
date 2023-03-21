namespace BigioHrServices.Model.Employee
{
    public class EmployeeAddRequest
    {
        public string NIK { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string JoinDate { get; set; } = string.Empty;
        public string WorkLength { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}
