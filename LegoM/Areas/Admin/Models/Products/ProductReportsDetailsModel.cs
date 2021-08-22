namespace LegoM.Areas.Admin.Models.Products
{
    using LegoM.Services.Reports.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductReportsDetailsModel
    {
        public ProductModel Product { get; set; }

        public IEnumerable<ReportServiceModel> Reports { get; set; }
    }
}
