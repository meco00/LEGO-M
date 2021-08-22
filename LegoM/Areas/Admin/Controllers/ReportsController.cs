namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Areas.Admin.Models.Reports;
    using LegoM.Services.Reports;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReportsController:AdminController
    {
        private readonly IReportsService reports;

        public ReportsController(IReportsService reports)
        {
            this.reports = reports;
        }

        public IActionResult All([FromQuery] ReportsQueryModel query)
        {
            var queryResult = this.reports.All(
         query.SearchTerm,
         query.CurrentPage,
         ReportsQueryModel.ReportsPerPage);

            query.Reports = queryResult.Reports;
            query.TotalReports = queryResult.TotalReports;

            return this.View(query);


        }


        public IActionResult Delete(int id)
        {
            var isDeleted = this.reports.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Comment was deleted successfully!";

            return RedirectToAction(nameof(All));
        }


    }
}
