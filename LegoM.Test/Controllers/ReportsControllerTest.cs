namespace LegoM.Test.Controllers
{
    using LegoM.Controllers;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Products;
    using static Data.Reports;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Reports;
    using LegoM.Data.Models;
    using LegoM.Models.Products;

    public  class ReportsControllerTest
    {

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyController<ReportsController>
                  .Instance(controller => controller
                       .WithUser()
                       .WithData(GetProduct()))
                  .Calling(c => c.Add(ProductTestId))
                 .ShouldHave()
                  .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .View();

       
         [Fact]
         public void GetAddShouldBeForAuthorizedUsersAndReturnNotFound()
          => MyController<ReportsController>
             .Instance(controller => controller
                  .WithUser()
                  .WithData(GetProduct(IsPublic: false)))
             .Calling(c => c.Add(ProductTestId))
            .ShouldHave()
             .ActionAttributes(attributes => attributes
             .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
             .NotFound();


          [Fact]
          public void GetAddShouldBeForAuthorizedUsersAndReturnBadRequest()
          => MyController<ReportsController>
             .Instance(controller => controller
                  .WithUser()
                  .WithData(GetProduct()).WithData(GetReports(1)))
             .Calling(c => c.Add(ProductTestId))
            .ShouldHave()
             .ActionAttributes(attributes => attributes
             .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .BadRequest();

          [Theory]
          [InlineData(ReportType.Nudity, TestContent)]
          public void PostAddShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
           ReportType reportType,
           string content)
           => MyController<ReportsController>
               .Instance(controller => controller
                         .WithUser()
                         .WithData(GetProduct()))
                .Calling(c => c.Add(ProductTestId, new ReportFormModel
                {
                    ReportType=reportType,
                    Content = content
         
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests()
                     .RestrictingForHttpMethod(HttpMethod.Post))
                 .ValidModelState()
                 .Data(data => data
                      .WithSet<Report>(set => set
                              .Any(x =>
                              x.ReportType == reportType &&
                              x.Content == content &&
                              x.UserId == TestUser.Identifier)))
                  .TempData(tempData => tempData
                           .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .AndAlso()
                  .ShouldReturn()
                   .Redirect(redirect => redirect
                         .To<ProductsController>(c => c
                          .Details(ProductTestId, With.Any<ProductsDetailsQueryModel>())));


           [Theory]
           [InlineData(ReportType.Nudity, TestContent)]
           public void PostAddShouldBeForAuthorizedUsersAndShoulReturnNotFoundWhenProductIsNotPublic(
           ReportType reportType,
           string content
           )
            => MyController<ReportsController>
             .Instance(controller => controller
                       .WithUser()
                       .WithData(GetProduct(IsPublic: false)))
           .Calling(c => c.Add(ProductTestId, new ReportFormModel
           {
               ReportType = reportType,
               Content = content

           }))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests()
                .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
             .ShouldReturn()
             .NotFound();


          [Theory]
          [InlineData(ReportType.Nudity, TestContent)]
          public void PostAddShouldBeForAuthorizedUsersAndShoulReturnBadRequestWhenUserAlreadyReportedProduct(
          ReportType reportType,
          string content
            )
          => MyController<ReportsController>
              .Instance(controller => controller
                  .WithUser()
                  .WithData(GetProduct()).WithData(GetReports(1)))
          .Calling(c => c.Add(ProductTestId, new ReportFormModel
          {
              ReportType = reportType,
              Content = content
         
          }))
          .ShouldHave()
          .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .BadRequest();
         



    }
}
