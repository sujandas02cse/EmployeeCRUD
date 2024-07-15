using AspNetCore.Reporting;
using EmployeeCRUD.Models;
using EmployeeCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;
using System.Text;

namespace EmployeeCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {

            var employees = await _employeeRepository.GetAllEmployee();
            return Ok(employees);
        }


        [HttpPost("GetEmployeeById")]
        public async Task<ActionResult<Employee>> GetEmployeeById([FromBody] IdRequest request) {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeRepository.GetEmployeeById(request.Id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }


        [HttpPost("DeleteEmployee")]
        public async Task<ActionResult<int>> DeleteEmployee([FromBody] IdRequest request)
        { 
            var result=await _employeeRepository.DeleteEmployeeById(request.Id);

            if (result> 0)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult<int>> AddEmployee([FromBody]Employee employee)
        {
        var result= await _employeeRepository.AddEmployee(employee);
            if (result > 0)
            {
                return Ok(result);
            }
            return BadRequest();
            
        }


        [HttpPost("UpdateEmployee")]

        public async Task<ActionResult<int>> UpdateEmployee([FromBody] Employee employee)
        { 
            var result=await _employeeRepository.UpdateEmployee(employee);
            if (result > 0)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("EmployeeReport")]
        public async Task<ActionResult> EmployeeReport()
        {
            // Ensure encoding provider is registered
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string wwwRootFolder = _webHostEnvironment.WebRootPath;
            string reportPath = Path.Combine(wwwRootFolder, @"Reports\rptEmployee.rdlc");

            // Create an empty DataTable if necessary
            DataTable empData = new DataTable();
            empData.Columns.Add("Id", typeof(int));
            empData.Columns.Add("Name", typeof(string));
            empData.Columns.Add("Position", typeof(string));

            var localReport = new LocalReport(reportPath);
            localReport.AddDataSource("dsEmployee", empData); // Add the empty DataTable as a data source

            var reportResult = localReport.Execute(RenderType.Pdf, 1, null);

            return File(reportResult.MainStream, MediaTypeNames.Application.Octet, "EmployeeRpt.pdf");


        }




    }
}
