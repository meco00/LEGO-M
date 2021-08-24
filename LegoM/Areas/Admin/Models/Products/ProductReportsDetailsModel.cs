namespace LegoM.Areas.Admin.Models.Products
{
    using LegoM.Services.Reports.Models;
    using System.Collections.Generic;

    public class ProductReportsDetailsModel
    {
        public ProductModel Product { get; set; }

        public IEnumerable<ReportServiceModel> Reports { get; set; }
    }
}
