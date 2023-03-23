using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Delegation;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    [Route("delegation-metrices")]
    [ApiController]
    public class DelegationController
    {
        private readonly IDelegationService _delegationService;
        private readonly string RequestNull = "Request cannot be null!";

        public DelegationController(IDelegationService delegationService)
        {
            _delegationService = delegationService;
        }

        [HttpGet()]
        public DatatableResponse GetDelegation([FromQuery] DelegationSearchRequest request)
        {
            if(request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _delegationService.GetList(request);
        }

        [HttpGet("{nik}")]
        public DelegationDetailResponse GetDetailDelegations(string nik)
        {
            if (nik == "") throw new Exception(RequestNull);

            return _delegationService.GetDetailDelegation(nik);
        }

        [HttpPost()]
        public BaseResponse AddDelegation([FromBody] DelegationAddRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            _delegationService.DelegationAdd(request);

            return new BaseResponse();
        }

        // [HttpPut("Update")]
        // public BaseResponse UpdateDelegation([FromBody] DelegationAddRequest request)
        // {
            
        // }
    }
}
