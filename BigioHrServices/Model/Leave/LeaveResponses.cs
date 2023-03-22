namespace BigioHrServices.Model.Leave
{
    public class LeaveResponse
    {
        public int Id { get; set; }
        public string StafNIK { get; set; } = string.Empty;
        public string DelegatedStafNIK { get; set; } = string.Empty;
        public string ReviewerNIK { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly LeaveDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
