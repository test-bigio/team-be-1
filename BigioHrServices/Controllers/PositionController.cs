using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Model.Position;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class PositionController
    {
        private readonly IPositionService _positionService;
        private readonly string RequestNull = "Request cannot be null!";

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet("positions")]
        public DatatableResponse GetPositions([FromQuery] PositionSearchRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _positionService.GetListPosition(request);
        }

        [HttpPost("positions")]
        public BaseResponse AddPosition([FromBody] PositionRequest request)
        {
            BaseResponse response = new BaseResponse();
            var getCodeExist = _positionService.GetPositionByCode(request.Code);

            if (request == null)
            {
                response.isSuccess = false;
                response.Message = "Request cannot be null!";
            }
            else if (getCodeExist == null)
            {
                response.isSuccess = false;
                response.Message = "Code already exist!";
            }
            else
            {
                _positionService.AddPosition(request);

                response.isSuccess = true;
                response.Message = "New Position Added!";
            }

            return new BaseResponse();
        }

        [HttpPut("positions")]
        public BaseResponse UpdateEmployee([FromBody] PositionRequest request)
        {
            BaseResponse response = new BaseResponse();
            var getCodeExist = _positionService.GetPositionByCode(request.Code);

            if (request == null)
            {
                response.isSuccess = false;
                response.Message = "Request cannot be null!";
            }
            else if (getCodeExist != null)
            {
                response.isSuccess = false;
                response.Message = "Code is not exist!";
            }
            else
            {
                _positionService.EditPosition(request);

                response.isSuccess = true;
                response.Message = "New Position Updated!";
            }

            return new BaseResponse();
        }
    }
}

