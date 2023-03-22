using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Leave;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BigioHrServices.Controllers
{
    [Route("leave")]
    [ApiController]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("requests")]
        public BaseResponse NewRequest(AddNewLeaveRequest request)
        {
            try
            {
                // todo validate pin token
                _leaveService.AddNewLeaveRequest(request);
                return new BaseResponse();
            }
            catch (Exception e)
            {
                return BaseResponse.FromException(e);
            }
        }

        [HttpPost("requests/{id}/approve")]
        public BaseResponse ApproveRequest(int id)
        {
            try
            {
                // todo validate pin token
                _leaveService.Approve(id);
                return new BaseResponse();
            }
            catch (Exception e)
            {
                return BaseResponse.FromException(e);
            }
        }

        [HttpPost("requests/{id}/reject")]
        public BaseResponse RejectRequest(int id)
        {
            try
            {
                // todo validate pin token
                _leaveService.Reject(id);
                return new BaseResponse();
            }
            catch (Exception e)
            {
                return BaseResponse.FromException(e);
            }
        }

        [HttpGet("quota/{id}")]
        public LeaveQuotaResponse GetLeaveQuota(string id)
        {
            return _leaveService.GetLeaveQuota(id);
        }

        [HttpGet("history/{id}")]
        public DatatableResponse GetLeaveHistory([FromQuery] LeaveHistoryRequest request, string id)
        {
            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

      return _leaveService.GetLeaveHistory(id, request);
    }

    [HttpGet("/requests")]
    public DatatableResponse ListRequests([FromQuery] LeaveSearchRequest request)
    {
        request.Page = request.Page <= 0 ? 1 : request.Page;
        request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;
        
        return _leaveService.GetList(request);
    }
  }
}
