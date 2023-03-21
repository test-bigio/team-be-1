using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.Employee
{
    public class EmployeeSearchRequest : DatatableRequest
    {
        public string JoinDateRangeBegin { get; set; } = string.Empty;
        public string JoinDateRangeEnd { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
