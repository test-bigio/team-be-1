using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigioHrServices.Controllers
{
    public class EmployeeController
    {
        private readonly IEmployeeService _employeeService;
        private readonly string RequestNull = "Request cannot be null!";

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("v1/user")]
        public Pageable<EmployeeResponse> GetEmployees([FromQuery] EmployeeSearchRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            request.Page = request.Page <= 0 ? 1 : request.Page;
            request.PageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            return _employeeService.GetList(request);
        }

        [HttpGet("v1/users/{nik}")]
        public EmployeeResponse GetDetailEmployees(string nik)
        {
            if (nik == null) throw new Exception(RequestNull);

            return _employeeService.GetDetailEmployees(nik);
        }


        [HttpPost("v1/users")]
        public BaseResponse AddEmployee([FromBody] EmployeeAddRequest request)
        {
            Console.WriteLine("test");
            if (request == null) throw new Exception(RequestNull);

            var getExisting = _employeeService.GetEmployeeByNIK(request.NIK);
            if (getExisting != null) throw new Exception("NIK already exist!");

            _employeeService.EmployeeAdd(request);

            return new BaseResponse();
        }

        [HttpPut("v1/users")]
        public BaseResponse UpdateEmployee([FromBody] EmployeeUpdateRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            _employeeService.EmployeeUpdate(request);

            return new BaseResponse();
        }

        [HttpPut("v1/users/non-active/{nik}")]
        public BaseResponse Delete(string nik)
        {
            var response = new BaseResponse();
            var data = _employeeService.GetEmployeeByNIK(nik);
            if (nik == null)
            {
                response.isSuccess = false;
                response.Message = RequestNull;
                return response;
            }
            else if (data == null)
            {
                response.isSuccess = false;
                response.Message = "NIK Tidak Ditemukan";
                return response;
            }
            else if (data.IsActive == false)
            {
                response.isSuccess = false;
                response.Message = "NIK Telah Non Active";
                return response;
            }

            _employeeService.EmployeeDelete(nik);
            response.isSuccess = true;
            response.Message = "Berhasil Mengnon-active pegawai dengan nik " + nik;
            return response;
        }
    }
}
