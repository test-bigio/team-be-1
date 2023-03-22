using BigioHrServices.Model;
using BigioHrServices.Model.AuditModule;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    public class AuditController
    {
        private readonly IAuditModuleServices _auditModuleServices;
        private readonly string RequestNull = "Request cannot be null!";

        public AuditController(IAuditModuleServices auditModuleServices)
        {
            _auditModuleServices = auditModuleServices;
        }

        [HttpGet("GetList")]
        public async Task<DatatableResponse> GetEmployees([FromQuery] AuditModuleDatatableRequest request)
        {
            if(request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return await _auditModuleServices.GetList(request);
        }
    }
}