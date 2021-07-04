using System;
using System.Data;
using System.IO;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PDFViewer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        readonly ILogger<ReportController> _logger;

        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string path = Directory.GetCurrentDirectory();

            var report = new Report();
            report.Load(Path.Combine(path, "App_Data/Master-Detail.frx"));

            var data = new DataSet();
            data.ReadXml("App_Data/nwind.xml");

            report.RegisterData(data, "NorthWind");

            report.Prepare();

            var pdfExport = new PDFSimpleExport();

            var now = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            var reportName = $"Report{now}";
            var reportPath = Path.Combine("file://",path, $"App_Data/Reports/{reportName}.pdf");

            pdfExport.Export(report, reportPath);

            var content = System.IO.File.ReadAllBytes(reportPath);
            return File(content, "application/pdf");
        }
    }
}