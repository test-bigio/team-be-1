using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.AuditModule;
using BigioHrServices.Model.Datatable;
using Microsoft.EntityFrameworkCore;

namespace BigioHrServices.Services
{
    public interface IAuditModuleServices
    {
        Task<DatatableResponse> GetList(AuditModuleDatatableRequest request);
        Task CreateLog(string module, string activity, string detail);
    }

    public class AuditModuleServices : IAuditModuleServices
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthUserService _authUserService;

        public AuditModuleServices(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, IAuthUserService authUserService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _authUserService = authUserService;
        }

        public async Task<DatatableResponse> GetList(AuditModuleDatatableRequest request)
        {
            var query = await _db.AuditModuls
                .AsNoTracking()
                .Select(x => (AuditModuleDto)x)
                .ToListAsync();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.IpAddress.ToLower() == request.Search.ToLower() ||
                    p.Username.ToLower() == request.Search.ToLower() ||
                    p.Module.ToLower() == request.Search.ToLower() ||
                    p.Activity.ToLower() == request.Search.ToLower() ||
                    p.Detail.ToLower() == request.Search.ToLower()).ToList();
            }

            if (!string.IsNullOrEmpty(request.Module))
            {
                query = query.Where(p => p.Module == request.Module).ToList();
            }

            return new DatatableResponse()
            {
                Data = query,
                TotalRecords = query.Count,
                PageSize = request.PageSize > query.Count ? query.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < query.Count,
                PrevPage = request.Page > 1,
            };
        }

        public async Task CreateLog(string module, string activity, string detail)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                string username = _authUserService.Name;

                AuditModul auditModul = new AuditModul()
                {
                    IpAddress = ipAddress,
                    Username = username,
                    Module = module,
                    Activity = activity,
                    Detail = detail
                };

                _db.AuditModuls.Add(auditModul);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
