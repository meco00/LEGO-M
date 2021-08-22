namespace LegoM.Test.Pipeline.Admin
{
    using LegoM.Areas.Admin;
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;

    using static Data.Reports;
    using static Data.Products;

    using ReportsController = Areas.Admin.Controllers.ReportsController;
    using LegoM.Areas.Admin.Models.Reports;

    public class ReportsPipelineTest
    {

        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
         => MyPipeline
             .Configuration()
             .ShouldMap(request => request
                 .WithPath("/Admin/Reports/All")
                  .WithUser(new[] { AdminConstants.AdministratorRoleName })
                  .WithAntiForgeryToken())
             .To<ReportsController>(c => c.All(With.Default<ReportsQueryModel>()))
             .Which(controller => controller
                 .WithData(GetProduct()).AndAlso().WithData(    GetReports()))
             .ShouldReturn()
             .View(view => view
                .WithModelOfType<ReportsQueryModel>()
              .Passing(model => model.Reports.Should().NotBeEmpty()));

        [Fact]
        public void DeleteShouldDeleteReportAndReturnRedirectToAll()
            => MyPipeline
             .Configuration()
              .ShouldMap(request => request
                .WithLocation($"/Admin/Reports/Delete/{1}")
                .WithUser(new[] { AdminConstants.AdministratorRoleName }))
              .To<ReportsController>(c => c
                    .Delete(1))
              .Which(controller => controller
                  .WithData(GetReports(1))
              .ShouldHave()
              .TempData(tempData => tempData
                   .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
              .Data(data => data.WithSet<Report>(set =>
              {
                  set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                  set.Should().BeEmpty();

              }))
              .AndAlso()
              .ShouldReturn()
               .Redirect(redirect => redirect
                      .To<ReportsController>(c => c
                      .All(With.Any<ReportsQueryModel>()))));


        [Fact]
        public void DeleteShouldReturnNotFoundWhenReportDoesNotExists()
           => MyPipeline
            .Configuration()
             .ShouldMap(request => request
               .WithLocation($"/Admin/Reports/Delete/{1}")
               .WithUser(new[] { AdminConstants.AdministratorRoleName }))
             .To<ReportsController>(c => c
                   .Delete(1))
             .Which()
             .ShouldReturn()
             .NotFound();



    }
}


