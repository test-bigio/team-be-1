using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.Position
{
    public class PositionSearchRequest : DatatableRequest
    {
        public bool IsActive { get; set; } = true;
    }
}
