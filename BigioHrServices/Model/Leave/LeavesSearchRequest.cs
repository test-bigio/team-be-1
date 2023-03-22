namespace BigioHrServices.Model.Leave
{
    public class LeaveSearchRequest : LeaveBaseRequest
    {
        public string stafNIK { get; set; } = string.Empty;
        public string reviewerNIK { get; set; } = string.Empty;
    }
}
