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

    using static Data.Products;
    using static Data.Users;
    using static Data.Categories;

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
        public void GetAddShouldBeForAuthorizedUsersReturnRedirectToBecomeMerchantWhenUserIsNotMerchant()
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





    }
}
