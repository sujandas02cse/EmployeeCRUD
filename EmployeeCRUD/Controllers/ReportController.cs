using Microsoft.AspNetCore.Mvc;
using ReportLibrary;

namespace EmployeeCRUD.Controllers
{
    public class ReportController : ControllerBase
    {

        private readonly IReportGenerator _reportGenerator;

        public ReportController(IReportGenerator reportGenerator)
        {
            _reportGenerator = reportGenerator;
        }

        [HttpGet("GetReport")]
        public IActionResult GetReport()
        {
            byte[] reportBytes = _reportGenerator.GenerateReport();
            return File(reportBytes, "application/pdf", "Report.pdf");
        }
    }
}
