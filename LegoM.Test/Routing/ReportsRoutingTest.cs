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
    using static Data.DataConstants;

   public class ReportsRoutingTest
    {
        [Fact]
        public void GetAddShouldBeMapped()
           => MyRouting
                .Configuration()
                 .ShouldMap($"/Reports/Add/{TestId}")
                  .To<ReportsController>(c => c.Add(TestId));


        [Fact]
        public void PostAddShouldBeMapped()
           => MyRouting
                .Configuration()
                 .ShouldMap(request=>request
                      .WithPath($"/Reports/Add/{TestId}")
                      .WithMethod(HttpMethod.Post))
                  .To<ReportsController>(c => c.Add(TestId,With.Any<ReportFormModel>()));



    }
}
