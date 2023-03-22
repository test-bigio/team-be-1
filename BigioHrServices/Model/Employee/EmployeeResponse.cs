namespace BigioHrServices.Model.Employee
{
    public class EmployeeResponse
    {
        public string NIK { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Sex { get; set; } = string.Empty;
        public string? JoinDate { get; set; }
        public string WorkLength { get; set; } = string.Empty;
        public string? PositionCode { get; set; }
        public string? Position { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? DigitalSignature { get; set; }
    }
}
