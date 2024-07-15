using Dapper;
using EmployeeCRUD.Data;
using EmployeeCRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace EmployeeCRUD.Service
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeCRUDContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeRepository(EmployeeCRUDContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<int> AddEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                var sql = "AddEmployee";
                return await connection.ExecuteAsync(sql,new {employee.EmployeeName,
                    employee.DOJ,employee.Department,employee.Company
                    
                },commandType:CommandType.StoredProcedure);
            }
        }

        public async Task<int> DeleteEmployeeById(int id)
        {
            using (var connection = new SqlConnection(context.ConnectionString)) {
                var sql = "DeleteEmployee";
                return await connection.ExecuteAsync(sql,new { Id=id},commandType:CommandType.StoredProcedure);
            }
        }

        public async Task<byte[]> GenerateEmployeeReport()
        {
            // Ensure encoding provider is registered
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string wwwRootFolder = _webHostEnvironment.WebRootPath;
            string reportPath = Path.Combine(wwwRootFolder, @"Reports\rptEmployee.rdlc");

            // Create the DataTable
            DataTable empData = new DataTable();
            using (SqlConnection connection = new SqlConnection(context.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(empData);
                }
            }

            var localReport = new LocalReport(reportPath);
            localReport.AddDataSource("dsEmployee", empData); // Add the filled DataTable as a data source

            var reportResult = localReport.Execute(RenderType.Pdf, 1, null);

            return reportResult.MainStream.ToArray();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                var sql = "GetAllEmployees"; // Your stored procedure name
                return await connection.QueryAsync<Employee>(sql, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                var sql = "GetEmployeeById";
                var employee= await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id }, commandType: CommandType.StoredProcedure);
                return employee;
            }
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                var sql = "UpdateEmployee";
                return await connection.ExecuteAsync(sql, new { employee.Id, employee.EmployeeName, employee.DOJ, employee.Department, employee.Company }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
