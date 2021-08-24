namespace LegoM.Test.Routing
{
    using LegoM.Controllers;
    using LegoM.Models.Reports;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using static Data.Products;

   public class ReportsRoutingTest
    {
        [Fact]
        public void GetAddShouldBeMapped()
           => MyRouting
                .Configuration()
                 .ShouldMap($"/Reports/Add/{ProductTestId}")
                  .To<ReportsController>(c => c.Add(ProductTestId));


        [Fact]
        public void PostAddShouldBeMapped()
           => MyRouting
                .Configuration()
                 .ShouldMap(request=>request
                      .WithPath($"/Reports/Add/{ProductTestId}")
                      .WithMethod(HttpMethod.Post))
                  .To<ReportsController>(c => c.Add(ProductTestId, With.Any<ReportFormModel>()));



    }
}
