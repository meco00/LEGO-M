namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Controllers;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;


    using static Data.Categories;
    using static Data.DataConstants;
    using static Data.Products;
    using static Data.Questions;
    using static Data.Reviews;
    using static Data.Users;

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
            => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithPath("/Products/Add")
               .WithUser())
               .To<ProductsController>(c => c.Add())
               .Which(controller => controller
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
        public void GetEditShouldBeForAuthorizedUsersReturnNotFoundWhenProductDoesNotExists()
           => MyPipeline
               .Configuration()
            .ShouldMap(request => request.WithPath($"/Products/Edit/{TestId}")
            .WithUser())
            .To<ProductsController>(c => c.Edit(TestId))
            .Which(controller => controller
                 .WithData(GetMerchant()))
            .ShouldHave()
                .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
              .AndAlso()
              .ShouldReturn()
              .NotFound();


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

        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersReturnNotFoundWhenProductDoesNotExists()
          => MyPipeline
              .Configuration()
           .ShouldMap(request => request.WithPath($"/Products/Delete/{TestId}")
           .WithUser())
           .To<ProductsController>(c => c.Delete(TestId))
           .Which(controller => controller
                .WithData(GetMerchant()))
           .ShouldHave()
               .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
             .NotFound();


        [Fact]
        public void DetailsShouldReturnViewWithCorrectDataAndModel()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                        .WithPath($"/Products/Details/{TestId}")
                 .WithUser())
                 .To<ProductsController>(c => c.Details(TestId))
                 .Which(controller => controller
                       .WithData(GetProduct())
                        .AndAlso()
                        .WithData(GetQuestionsByProduct())
                        .AndAlso()
                        .WithData(GetReviewsByProduct()))
             .ShouldReturn()
             .View(view => view.WithModelOfType<ProductDetailsModel>()
              .Passing(model =>
              {
                  model.Product.Should().NotBeNull();
                  model.ProductReviewsStatistics.Should().NotBeNull();
                  model.SimilarProducts.Should().NotBeNull();
                  model.Questions.Should().HaveCount(5);
                  model.Reviews.Should().HaveCount(5);

              }));

        [Fact]
        public void DetailsShouldReturnNotFoundWhenProductDoesNotExists()
          => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                      .WithPath($"/Products/Details/{TestId}")
               .WithUser())
               .To<ProductsController>(c => c.Details(TestId))
               .Which()
               .ShouldReturn()
               .NotFound();



        [Fact]
        public void DetailsShouldReturnBadRequestWhenProductIsNotPublic()
        => MyPipeline
             .Configuration()
             .ShouldMap(request => request
                    .WithPath($"/Products/Details/{TestId}")
             .WithUser())
             .To<ProductsController>(c => c.Details(TestId))
             .Which(controller => controller.WithData(GetProduct(TestId, false, false, false))
             .ShouldReturn()
             .BadRequest());


        [Fact]
        public void AllShouldReturnDefaultViewWithCorrectModel()
            => MyPipeline
             .Configuration()
             .ShouldMap(request => request
                    .WithPath($"/Products/All"))
                .To<ProductsController>(c => c.All(With.Default<ProductsQueryModel>()))
               .Which(controller => controller
                   .WithData(GetProducts(10, false, false))
                   .AndAlso()
                   .WithData(GetCategories()))
                .ShouldReturn()
               .View(view => view
                    .WithModelOfType<ProductsQueryModel>()
                     .Passing(model =>
                     {
                         model.Products.Should().HaveCount(6);
                         model.CurrentPage.Should().Be(1);
                         model.TotalProducts.Should().Be(10);
                         model.ProductSorting.Should().Be(ProductSorting.Default);
                         model.Categories.Should().HaveCount(5);
                         model.SubCategories.Should().HaveCount(5);
                     }));


        [Theory]
        [InlineData("TestCategory", "TestSubCategory", 0, "Title")]
        public void AllWithCategorySubCategoryAndSearchTermShouldReturnDefaultViewWithCorrectModel(
            string categoryName,
            string subCategoryName,
            int productSorting,
            string searchTerm)
          => MyPipeline
           .Configuration()
           .ShouldMap(request => request
           .WithLocation($"/Products/All?Category={categoryName}&SubCategory={subCategoryName}&ProductSorting={productSorting}&SearchTerm={searchTerm}"))
              .To<ProductsController>(c => c.All(new ProductsQueryModel 
              {
                  Category= categoryName,
                  SubCategory= subCategoryName,
                  SearchTerm= searchTerm,
                  ProductSorting= (ProductSorting)productSorting,
              }))
             .Which(controller => controller
                 .WithData(GetProduct()))
              .ShouldReturn()
             .View(view => view
                  .WithModelOfType<ProductsQueryModel>()
                   .Passing(model =>
                   {
                       model.Products.Should().HaveCount(1);
                       model.CurrentPage.Should().Be(1);
                       model.TotalProducts.Should().Be(1);
                       model.ProductSorting.Should().Be((ProductSorting)productSorting);
                       model.Category.Should().Be(categoryName);
                       model.SubCategory.Should().Be(subCategoryName);

                   }));

        [Theory]
        [InlineData(12,2)]
        public void GetAllWithPageShouldReturnDefaultViewWithCorrectModel(
            int productsCount,
            int page)
          => MyPipeline
           .Configuration()
           .ShouldMap(request => request
           .WithLocation($"/Products/All?CurrentPage={page}"))
              .To<ProductsController>(c => c.All(new ProductsQueryModel
              {
                  CurrentPage=page

              }))
             .Which(controller => controller
                 .WithData(GetProducts(productsCount)))
              .ShouldReturn()
               .View(view => view
                  .WithModelOfType<ProductsQueryModel>()
                   .Passing(model =>
                   {
                       model.Products.Should().HaveCount(productsCount/2);
                       model.CurrentPage.Should().Be(page);
                      

                   }));






        [Fact]
        public void AllShouldReturnBadRequestWhenCategorySubCategoryConditionIsInvalid()
            => MyMvc
            .Pipeline()
              .ShouldMap(request => request
        .WithLocation($"/Products/All?Category=&SubCategory={"TestSubCategory"}&ProductSorting=0&SearchTerm="))
               .To<ProductsController>(c => c.All(new ProductsQueryModel
               {
                   SubCategory = "TestSubCategory"
               }))
               .Which(controller => controller
                   .WithData(GetCategories()))
                .ShouldReturn()
                .BadRequest();








    }
}
