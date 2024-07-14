using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary
{
    public class ReportGenerator:IReportGenerator
    {
        public byte[] GenerateReport() {

            LocalReport report = new LocalReport();
            report.ReportPath = "Reports/rptShowAllEmployee.rdlc";

            string mimeType, encoding, extension;
            Warning[] warnings;
            string[] streamids;
            byte[] bytes = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }
    }
}
