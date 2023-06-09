﻿namespace BigioHrServices.Model.Employee
{
    public class EmployeeUpdateRequest
    {
        public string NIK { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string JoinDate { get; set; } = string.Empty;
        public string WorkLength { get; set; } = string.Empty;
        public string PositionId { get; set; }
        public bool IsOnLeave { get; set; } = false;
        public string Email { get; set; } = string.Empty;
    }
}
