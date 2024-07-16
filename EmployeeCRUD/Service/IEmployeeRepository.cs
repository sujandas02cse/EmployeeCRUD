using EmployeeCRUD.Models;

namespace EmployeeCRUD.Service
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployee();
        Task<Employee> GetEmployeeById(int id);
        Task<int> AddEmployee(Employee employee);
        Task<int> UpdateEmployee(Employee employee);
        Task<int> DeleteEmployeeById(int id);
        Task<byte[]> GenerateEmployeeReport();
    }
}
