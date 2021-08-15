namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Controllers;
    using LegoM.Models.Products;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.DataConstants;
    using static Data.Products;
    using static Data.Users;
    using static Data.Categories;
    using LegoM.Areas.Admin;

    public class ProductsPipelineTest
    {
        [Fact]
        public void MineShouldBeForAuthorizedUsersReturnViewWithCorrectDataAndModel()
        => MyPipeline
             .Configuration()
             .ShouldMap(request => request.WithPath("/Products/Mine")
             .WithUser())
            .To<ProductsController>(c => c.Mine())
            .Which(controller => controller.WithData(GetProducts()))
            .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                     .WithModelOfType<List<ProductServiceModel>>()
                     .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void MineShouldRedirectToAllWhenUserIsAdmin()
        => MyPipeline
              .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Products/Mine")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<ProductsController>(c => c.Mine())
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                              .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<ProductsController>(c => c
                   .All(With.Any<ProductsQueryModel>())));

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersReturnViewAndCorrectModel()
            =>MyPipeline
               .Configuration()
               .ShouldMap(request=>request.WithPath("/Products/Add")
               .WithUser())
               .To<ProductsController>(c=>c.Add())
               .Which(controller=>controller
                   .WithData(GetMerchant())
                   .AndAlso()
                   .WithData(GetCategories()))
               .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                  .View(view => view
                     .WithModelOfType<ProductFormModel>()
                     .Passing(model =>
                     {
                         model.Categories.Should().HaveCount(5);
                         model.SubCategories.Should().HaveCount(5);

                      }));


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnRedirectToBecomeMerchantWhenUserIsNotMerchant()
            => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithPath("/Products/Add")
               .WithUser())
               .To<ProductsController>(c => c.Add())
               .Which()
               .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<MerchantsController>(c => c
                      .Become()));


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersReturnViewWithCorrectDataAndModel()
          => MyPipeline
              .Configuration()
               .ShouldMap(request => request.WithPath($"/Products/Edit/{TestId}")
               .WithUser())
              .To<ProductsController>(c => c.Edit(TestId))
            .Which(controller => controller
                .WithData(GetProduct())
           .ShouldHave()
               .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
            .View(view => view
                 .WithModelOfType<ProductFormModel>()));

        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotMerchant()
          => MyPipeline
                .Configuration()
             .ShouldMap(request => request.WithPath($"/Products/Edit/{TestId}")
             .WithUser())
             .To<ProductsController>(c => c.Edit(TestId))
             .Which()
             .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest();


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersReturnBadRequestWhenProductIsNotOfUser()
            => MyPipeline
                .Configuration()
             .ShouldMap(request => request.WithPath($"/Products/Edit/{TestId}")
             .WithUser())
             .To<ProductsController>(c => c.Edit(TestId))
             .Which(controller => controller
                  .WithData(GetMerchant())
                  .AndAlso()
                  .WithData(GetProduct(TestId, false))
             .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest());


        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request.WithPath($"/Products/Delete/{TestId}")
                   .WithUser())
             .To<ProductsController>(c => c.Delete(TestId))
             .Which(controller => controller
                  .WithData(GetProduct())
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View());



        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotMerchant()
       => MyPipeline
             .Configuration()
          .ShouldMap(request => request.WithPath($"/Products/Delete/{TestId}")
          .WithUser())
          .To<ProductsController>(c => c.Delete(TestId))
          .Which()
          .ShouldHave()
              .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .BadRequest();



        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersReturnBadRequestWhenProductIsNotOfMerchant()
            => MyPipeline
                .Configuration()
             .ShouldMap(request => request.WithPath($"/Products/Delete/{TestId}")
             .WithUser())
             .To<ProductsController>(c => c.Delete(TestId))
             .Which(controller => controller
                  .WithData(GetMerchant())
                  .AndAlso()
                  .WithData(GetProduct(TestId, false))
             .ShouldHave()
                 .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .BadRequest());



















    }
}
