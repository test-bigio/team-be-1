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
            //_logactivityService = logactivityService;
        }

        [HttpGet("v1/Users")]
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


        [HttpPost("Add")]
        public BaseResponse AddEmployee([FromBody] EmployeeAddRequest request)
        {
            Console.WriteLine("test");
            if (request == null) throw new Exception(RequestNull);

            var getExisting = _employeeService.GetEmployeeByNIK(request.NIK);
            if (getExisting != null) throw new Exception("NIK already exist!");

            _employeeService.EmployeeAdd(request);

            return new BaseResponse();
        }

        [HttpPut("Update")]
        public BaseResponse UpdateEmployee([FromBody] EmployeeUpdateRequest request)
        {
            if (request == null) throw new Exception(RequestNull);

            _employeeService.EmployeeUpdate(request);

            return new BaseResponse();
        }

        [HttpPut("Delete")]
        public BaseResponse Delete([FromForm] string nik)
        {
            if (nik == null) throw new Exception(RequestNull);
            _employeeService.EmployeeDelete(nik);
            return new BaseResponse();
        }
    }
}
