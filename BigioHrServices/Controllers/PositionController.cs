using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Position;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
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

        [HttpGet("positions/{id}")]
        public Position GetDetailPosition(string id)
        {
            return _positionService.GetDetailPosition(id);
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
        public BaseResponse UpdatePosition([FromBody] PositionRequest request)
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
                response.Message = "Position is Updated!";
            }

            return new BaseResponse();
        }

        [HttpDelete("positions/{id}")]
        public BaseResponse DeletePosition(string id)
        {
            BaseResponse response = new BaseResponse();
            var getCodeExist = _positionService.GetPositionByCode(id);
            
            if (getCodeExist != null)
            {
                response.isSuccess = false;
                response.Message = "Code is not exist!";
            }
            else
            {
                _positionService.InactivePosition(id);

                response.isSuccess = true;
                response.Message = "Position is Inactive!";
            }

            return new BaseResponse();
        }
    }
}

