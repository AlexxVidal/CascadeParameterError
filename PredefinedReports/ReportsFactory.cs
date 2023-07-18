using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;

namespace ErrorSample.PredefinedReports
{
    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            ["ReportReport"] = () => new ReportReport(),
        };
    }
}