namespace LegoM.Test.Controllers.Admin
{
    using FluentAssertions;
    using LegoM.Data.Models;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    using static Data.Products;

    using static LegoM.Areas.Admin.AdminConstants;

    using AdminController = LegoM.Areas.Admin.Controllers.AdminController;

    using ProductsController = LegoM.Areas.Admin.Controllers.ProductsController;



    public class ProductControllerTest
    {
        [Fact]
        public void ControllerShouldBeInAdminArea()
            => MyController<AdminController>
                .ShouldHave()
                 .Attributes(attributes => attributes
                     .SpecifyingArea(AreaName)
                     .RestrictingForAuthorizedRequests(AdministratorRoleName));



        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<ProductsController>
                .Instance(controller => controller
                .WithData(GetProducts(3)))
                .Calling(x => x.All())
                 .ShouldReturn()
                 .View(view => view.WithModelOfType<List<ProductServiceModel>>()
                 .Passing(model => model.Should().HaveCount(3)));

        [Fact]
        public void DeletedShouldReturnCorrectViewWithModel()
            => MyController<ProductsController>
               .Instance(controller => controller
                .WithData(GetProducts(3,true)))
                .Calling(x => x.Deleted())
                .ShouldReturn()
                .View(view => view.WithModelOfType<List<ProductDeletedServiceModel>>()
                .Passing(model => model.Should().NotBeEmpty()));


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ChangeVisibilityShouldChangeProductAndRedirectToAll(
            bool IsPublic)
            => MyController<ProductsController>
             .Instance(controller => controller
                .WithData(GetProduct("TestId", false,IsPublic)))
              .Calling(x => x.ChangeVisibility("TestId"))
              .ShouldHave()
               .Data(data => data
                   .WithSet<Product>(set =>
               {
                   var product = set.Find("TestId");

                   product.Should().NotBeNull();

                   product.IsPublic.Should().Be(!IsPublic);

               }))
              .AndAlso()
            .ShouldReturn()
            .RedirectToAction("All");



        [Fact]
        public void ReviveShouldChangeProductAndRedirectToAll()
          => MyController<ProductsController>
              .Instance(controller => controller
                 .WithData(GetProduct("TestId",true)))
              .Calling(x => x.Revive("TestId"))
              .ShouldHave()
                .Data(data => data
                   .WithSet<Product>(set =>
                   {
                       var product = set.Find("TestId");

                       product.Should().NotBeNull();

                       product.IsDeleted.Should().Be(false);

                   }))
                  .AndAlso()
                  .ShouldReturn()
                  .RedirectToAction("All");



    }
}
