namespace BigioHrServices.Model.Leave
{
    public enum SortBy
    {
        CreatedAt,
        CreatedAt_DESC,
        LeaveStart,
        LeaveStart_DESC,
    }

    public class LeaveBaseRequest
    {
        public string Search {get; set;} = string.Empty;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SortBy SortBy { get; set; }
    }
}
