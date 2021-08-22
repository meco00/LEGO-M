namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Areas.Admin.Models.Products;
    using LegoM.Data.Models;
    using LegoM.Models.Products;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    using static Data.DataConstants;
    using static Data.Products;
    using static Data.Reports;

    using ProductsController = LegoM.Areas.Admin.Controllers.ProductsController;


    public class ProductsPipelineTest
    {

        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request=>request
                    .WithPath("/Admin/Products/Existing")
                     .WithUser( new[] {AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<ProductsController>(c => c.Existing(With.Default<ProductsQueryModel>()))
                .Which(controller => controller
                    .WithData(GetProducts()))              
                .ShouldReturn()
            .View(view => view.WithModelOfType<ProductsQueryModel>()
               .Passing(model => model.Products.Should().NotBeEmpty()));



        [Fact]
        public void DeletedShouldReturnCorrectViewAndModel()
             => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Products/Deleted")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<ProductsController>(c => c.Deleted(With.Default<ProductsQueryModel>()))
                .Which(controller => controller
                    .WithData(GetProducts(3,true)))
                .ShouldReturn()
            .View(view => view.WithModelOfType<ProductsQueryModel>()
               .Passing(model => model.Products.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeProductAndRedirectToAll()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                    .WithPath($"/Admin/Products/ChangeVisibility/{TestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                   .To<ProductsController>(c => c.ChangeVisibility(TestId))
                   .Which(controller => controller
                    .WithData(GetProduct()))
                   .ShouldHave()
                    .Data(data => data
                         .WithSet<Product>(set => set
                              .Any(x => x.Id == TestId && !x.IsPublic)))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<ProductsController>(c =>c.Existing(With.Any<ProductsQueryModel>())));
                    


        [Fact]
        public void ReviveShouldChangeProductAndRedirectToAll()
            => MyPipeline
                .Configuration()
                .ShouldMap(request=>request
                     .WithPath($"/Admin/Products/Revive/{TestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<ProductsController>(c=>c.Revive(TestId))
                .Which(controller=>controller
                    .WithData(GetProduct(TestId,true)))
                 .ShouldHave()
                  .Data(data => data
                         .WithSet<Product>(set => set
                              .Any(x => x.Id == TestId && !x.IsDeleted)))
                    .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                         .To<ProductsController>(c => c.Existing(With.Any<ProductsQueryModel>())));


        [Fact]
        public void ReportShouldReturnViewWithCorectModelAndData()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                     .WithPath($"/Admin/Products/Reports/{TestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                .To<ProductsController>(c => c.Reports(TestId))
                .Which(controller => controller
                    .WithData(GetProduct()).AndAlso().WithData(GetReports(5,sameUser:false)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ProductReportsDetailsModel>()
                    .Passing(model =>
                    {
                        model.Product.Title.Should().Be("Title");
                        model.Product.Image.Should().Be("TestUrl");
                        model.Reports.Should().HaveCount(5);
                    }));


        [Fact]
        public void ReportShouldReturnNotFoundWhenProductDoesNotExists()
           => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                    .WithPath($"/Admin/Products/Reports/{TestId}")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<ProductsController>(c => c.Reports(TestId))
               .Which()
               .ShouldReturn()
               .NotFound();



    }
}
