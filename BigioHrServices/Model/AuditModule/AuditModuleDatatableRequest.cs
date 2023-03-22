using BigioHrServices.Model.Datatable;

namespace BigioHrServices.Model.AuditModule
{
    public class AuditModuleDatatableRequest : DatatableRequest
    {
        public string? Module { get; set; }
    }
}
