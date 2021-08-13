namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Areas.Admin.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static Data.DataConstants;
    using static Data.Products;

    public class ProductsControllerTest
    {

        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request=>request
                    .WithPath("/Admin/Products/All")
                     .WithUser( new[] {AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<ProductsController>(c => c.All())
                .Which(controller => controller
                    .WithData(TenPublicProducts()))              
                .ShouldReturn()
            .View(view => view.WithModelOfType<List<ProductServiceModel>>()
               .Passing(model => model.Should().NotBeEmpty()));



        [Fact]
        public void DeletedShouldReturnCorrectViewAndModel()
             => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Products/Deleted")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                .To<ProductsController>(c => c.Deleted())
                .Which(controller => controller
                    .WithData(TenDeletedProducts()))
                .ShouldReturn()
            .View(view => view.WithModelOfType<List<ProductDeletedServiceModel>>()
               .Passing(model => model.Should().NotBeEmpty()));


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
                        .To<ProductsController>(c => c.All()));
                    


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
                    .WithData(GetDeadProduct()))
                 .ShouldHave()
                  .Data(data => data
                         .WithSet<Product>(set => set
                              .Any(x => x.Id == TestId && !x.IsDeleted)))
                    .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                         .To<ProductsController>(c => c.All()));

    }
}
