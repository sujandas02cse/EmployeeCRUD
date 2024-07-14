namespace EmployeeCRUD.Service
{
    public interface IReportService
    {
        Task<byte[]> GenerateEmployeeReport();
    }
}
