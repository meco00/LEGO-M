namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Models.Reports;
    using LegoM.Services.Products;
    using LegoM.Services.Reports;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ReportsController:Controller
    {
        private readonly IProductsService products;
        private readonly IReportsService reports;

        public ReportsController(IProductsService products, IReportsService reports)
        {
            this.products = products;
            this.reports = reports;
        }

        [Authorize]
        public IActionResult Add(string id)
        {
            if (!this.products.IsProductPublic(id))
            {
                return NotFound();
            }

            if (this.reports.ReportExistsForProduct(id,this.User.Id()))
            {
                return BadRequest();
            }

            return View();

        }


        [Authorize]
        [HttpPost]
        public IActionResult Add(string id,ReportFormModel report)
        {
            if (!this.products.IsProductPublic(id))
            {
                return NotFound();
            }

            if (this.reports.ReportExistsForProduct(id, this.User.Id()))
            {
                return BadRequest();
            }

            if (!(ModelState.IsValid))
            {
                return this.View(report);

            }

            this.reports.Add(
                report.Content,
                report.ReportType.Value,
                id,
                this.User.Id());

            this.TempData[WebConstants.GlobalMessageKey] = $"Report was added and send to administration!";

            return RedirectToAction(nameof(ProductsController.Details), "Products", new { id });
        }

    }
}
