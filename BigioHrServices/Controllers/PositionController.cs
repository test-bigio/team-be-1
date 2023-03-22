using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model.Position;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;
using MessagePack;

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

        [HttpGet("positions/{code}")]
        public Position GetDetailPosition(string code)
        {
            return _positionService.GetDetailPosition(code);
        }

        [HttpPost("positions")]
        public BaseResponse AddPosition([FromBody] PositionAddRequest request)
        {
            var response = new BaseResponse();

            if (request == null)
            {
                response.isSuccess = false;
                response.Message = RequestNull;
                return response;
            }

            _positionService.AddPosition(request);
            response.isSuccess = true;
            response.Message = "Berhasil Menambahkan jabatan";

            return response;  
        }

        [HttpPut("positions")]
        public BaseResponse UpdatePosition([FromBody] PositionEditRequest request)
        {
            var response = new BaseResponse();
            var data = _positionService.GetPositionByCode(request.Code);

            if (request == null)
            {
                response.isSuccess = false;
                response.Message = RequestNull;
                return response;
            }
            else if (data == null)
            {
                response.isSuccess = false;
                response.Message = "Code Tidak Ditemukan";
                return response;
            }
            else if (data.IsActive == false)
            {
                response.isSuccess = false;
                response.Message = "Code Saat ini Non Active";
                return response;
            }

            _positionService.EditPosition(request);
            response.isSuccess = true;
            response.Message = "Berhasil Mengupdate jabatan dengan code " + request.Code;

            return response;
        }

        [HttpDelete("positions/non-active/{code}")]
        public BaseResponse DeletePosition(string code)
        {
            var response = new BaseResponse();
            var data = _positionService.GetPositionByCode(code);

            if (code == null)
            {
                response.isSuccess = false;
                response.Message = RequestNull;
                return response;
            }
            else if (data == null)
            {
                response.isSuccess = false;
                response.Message = "Code Tidak Ditemukan";
                return response;
            }
            else if (data.IsActive == false)
            {
                response.isSuccess = false;
                response.Message = "Code Telah Non Active";
                return response;
            }

            _positionService.InactivePosition(code);
            response.isSuccess = true;
            response.Message = "Berhasil Mengnon-active jabatan dengan code " + code;

            return response;
        }
    }
}

