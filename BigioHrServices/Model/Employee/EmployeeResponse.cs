﻿namespace BigioHrServices.Model.Employee
{
    public class EmployeeResponse
    {
        public string NIK { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public DateOnly JoinDate { get; set; } = new DateOnly();
        public string WorkLength { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string DigitalSignature { get; set; } = "101010";
    }
}
