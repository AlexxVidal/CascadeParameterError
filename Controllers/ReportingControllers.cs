using DevExpress.Compatibility.System.Web;
using DevExpress.DataAccess.Sql;
using System.Collections.Generic;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;
using DevExpress.DataAccess.Json;
using System;

namespace ErrorSample.Controllers
{
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }

    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService)
        {
        }

        [HttpPost("[action]")]
        public IActionResult GetDesignerModel([FromForm] string reportUrl, [FromServices] IReportDesignerClientSideModelGenerator modelGenerator)
        {
            var dataSources = new Dictionary<string, object>();
            var ds = new SqlDataSource("NWindConnectionString");

            // Configure the SQL data source
            ds.ConnectionName = "NWindConnectionString";

            //// Add the custom queries
            //ds.Queries.Add(new CustomSqlQuery("BranchesParameterQuery", "SELECT \"BranchId\", \"Name\" FROM \"dbo\".\"Branch\""));
            ////ds.Queries.Add(new CustomSqlQuery("ClosingsParameterQuery", "SELECT \"PointOfSaleId\", \"BranchId\", \"ClosingId\", \"ClosingNumber\", \"EndDate\", \"PointOfSaleNumber\", \"Name\" FROM \"dbo\".\"Closing\" INNER JOIN \"dbo\".\"PointOfSale\" ON \"PointOfSale\".\"BranchId\" = \"Closing\".\"BranchId\" AND \"PointOfSale\".\"PointOfSaleId\" = \"Closing\".\"PointOfSaleId\" WHERE (NOT ((\"EndDate\") IS NULL OR LEN(\"EndDate\") = 0)) AND ((@PeriodStartQueryParameter <= \"EndDate\") AND (\"EndDate\" <= @PeriodEndQueryParameter)) ORDER BY \"EndDate\" DESC"));
            //ds.Queries.Add(new CustomSqlQuery("PointOfSalesParameterQuery", "SELECT \"BranchId\", \"PointOfSaleId\", \"Name\" FROM \"dbo\".\"PointOfSale\""));
            //ds.Queries.Add(new CustomSqlQuery("ProductCategoriesQuery", "SELECT \"ProductCategoryId\", \"Name\" FROM \"dbo\".\"ProductCategory\""));

            ds.Fill();

            dataSources.Add("sqlDataSource1", ds);

            var model = modelGenerator.GetModel(reportUrl, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            return DesignerModel(model);
        }
    }

    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
}