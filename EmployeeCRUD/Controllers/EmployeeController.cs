using EmployeeCRUD.Models;
using EmployeeCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

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


       



    }
}
