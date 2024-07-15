using AspNetCore.Reporting;
using EmployeeCRUD.Data;
using EmployeeCRUD.Models;
using EmployeeCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
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
        private readonly EmployeeCRUDContext context;

        public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment, EmployeeCRUDContext context)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
            this.context = context;
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
            var reportBytes = await _employeeRepository.GenerateEmployeeReport();

            if (reportBytes == null || reportBytes.Length == 0)
            {
                return NoContent(); // or handle the empty result case as needed
            }
            return File(reportBytes, MediaTypeNames.Application.Octet, "EmployeeRpt.pdf");
        }




    }
}
