using Dapper;
using EmployeeCRUD.Data;
using EmployeeCRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace EmployeeCRUD.Service
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeCRUDContext context;

        public EmployeeRepository(EmployeeCRUDContext context)
        {
            this.context = context;
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

        public Task<int> UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
