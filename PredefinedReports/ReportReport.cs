using DevExpress.XtraReports.UI;

namespace ErrorSample.PredefinedReports
{
    public class ReportReport : XtraReport
    {
        public ReportReport()
        {
            string reportFilePath = @"Reports/ReportReport.repx";
            LoadLayoutFromXml(reportFilePath);
        }
    }
}