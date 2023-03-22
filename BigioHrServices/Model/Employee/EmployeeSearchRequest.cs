using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.Employee
{
    public class EmployeeSearchRequest : DatatableRequest
    {
        public string NIK { get; set; }
        public string JoinDateRangeBegin { get; set; }
        public string JoinDateRangeEnd { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
