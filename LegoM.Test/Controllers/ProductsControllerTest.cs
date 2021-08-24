namespace LegoM.Test.Controllers
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    using static Data.Categories;
    using static Data.Merchants;
    using static Data.Products;

    public class ProductsControllerTest
    {
        [Fact]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectToAndCorrectData()
            => MyController<ProductsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetMerchant())
                         .WithData(GetCategories()))
                 .Calling(c => c.Add(new ProductFormModel
                 {
                     Title = "TitleTest",
                     Description = "DescriptionTest",
                     Price = 12.50M,
                     Quantity = 2,
                     FirstImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Cat_img.jpg",
                     CategoryId = 1,
                     SubCategoryId = 1,
                     Condition = ProductCondition.New,
                     Delivery = DeliveryTake.Seller,
                     AgreeOnTermsOfPolitics=true

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Product>(set => set
                             .Any(x =>
                             x.Title == "TitleTest" &&
                             x.Description == "DescriptionTest" &&
                             x.Price == 12.50M &&
                             x.Quantity == 2 &&
                             x.Images.Any() &&
                             x.CategoryId == 1 &&
                             x.SubCategoryId == 1 &&
                             x.IsPublic==false &&
                             x.ProductCondition == ProductCondition.New &&
                             x.DeliveryTake == DeliveryTake.Seller)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                       .To<ProductsController>(c => c
                       .Details(With.Any<string>(),With.Any<ProductsDetailsQueryModel>())));


        [Fact]
        public void PostAddShouldReturnViewWhenFormIsWithWrongFields()
            => MyController<ProductsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetMerchant()))
                 .Calling(c => c.Add(new ProductFormModel
                 {
                    

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .Data(data => data
                     .WithSet<Product>(set => set.Should().BeEmpty()))
                 .AndAlso()
                 .ShouldReturn()
                 .View(view => view
                      .WithModelOfType<ProductFormModel>());

        [Fact]
        public void PostAddShouldReturnBadRequestWhenUserIsNotMerchant()
           => MyController<ProductsController>
               .Instance(controller => controller
                       .WithUser())
                .Calling(c => c.Add(new ProductFormModel
                {


                }))
               .ShouldHave()
              .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests()
                   .RestrictingForHttpMethod(HttpMethod.Post))
                 .AndAlso()
                .ShouldReturn()
                 .BadRequest();


        [Theory]
        [InlineData(
            "TitleTest",
            "DescriptionTest", 
            12.55,
            2,
            FirstImageUrl,
            SecondImageUrl,
            ThirdImageUrl,
            2,
            1,
            ProductCondition.New,
            DeliveryTake.Seller
            )]
        public void PostEditShouldEditProductAndReturnRedirectToWithCorrectDataAndModel(
            string title,
            string description,
            decimal price,
            byte quantity,
            string firstImageUrl,
            string secondImageUrl,
            string thirdImageUrl,
            int categoryId,
            int subCategoryId,
            ProductCondition condition,
            DeliveryTake deliveryTake)
            => MyController<ProductsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetProduct()))
                 .Calling(c => c.Edit(ProductTestId, new ProductFormModel
                 {
                     Title = title,
                     Description = description,
                     Price = price,
                     Quantity = quantity,
                     FirstImageUrl = firstImageUrl,
                     SecondImageUrl= secondImageUrl,
                     ThirdImageUr= thirdImageUrl,
                     CategoryId = categoryId,
                     SubCategoryId = subCategoryId,
                     Condition = condition,
                     Delivery = deliveryTake,

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Product>(set => set
                             .Any(x =>
                             x.Title == title &&
                             x.Description == description &&
                             x.Price == price &&
                             x.Quantity == quantity &&
                             x.Images.Any() &&
                             x.CategoryId == categoryId &&
                             x.SubCategoryId == subCategoryId &&
                             x.IsPublic == false &&
                             x.ProductCondition == condition &&
                             x.DeliveryTake == deliveryTake)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                       .To<ProductsController>(c => c
                       .Details(ProductTestId, With.Any<ProductsDetailsQueryModel>())));




        [Fact]
        public void PostEditShouldReturnBadRequestWhenUserIsNotMerchant()
         => MyController<ProductsController>
             .Instance(controller => controller
                     .WithUser())
              .Calling(c => c.Edit(ProductTestId, new ProductFormModel
              {


              }))
             .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
               .AndAlso()
              .ShouldReturn()
               .BadRequest();

        [Fact]
        public void PostEditShouldReturnNotFoundWhenProductDoesNotExists()
      => MyController<ProductsController>
          .Instance(controller => controller
                  .WithUser(new[] {AdminConstants.AdministratorRoleName }))
           .Calling(c => c.Edit(ProductTestId, With.Any<ProductFormModel>()))
          .ShouldHave()
         .ActionAttributes(attributes => attributes
             .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
           .ShouldReturn()
            .NotFound();


        [Fact]
        public void PostEditShouldReturnBadRequestWhenProductIsNotOfUser()
       => MyController<ProductsController>
           .Instance(controller => controller
                   .WithUser()
                   .WithData(GetMerchant())
                   .WithData(GetProduct(ProductTestId, false)))
            .Calling(c => c.Edit(ProductTestId, new ProductFormModel
            {


            }))
           .ShouldHave()
          .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldReturn()
             .BadRequest();


        [Fact]
        public void PostEditShouldReturnViewWhenFormFieldsAreWrong()
   => MyController<ProductsController>
       .Instance(controller => controller
               .WithUser()
               .WithData(GetProduct()))
        .Calling(c => c.Edit(ProductTestId, new ProductFormModel
        {


        }))
       .ShouldHave()
      .ActionAttributes(attributes => attributes
          .RestrictingForAuthorizedRequests()
           .RestrictingForHttpMethod(HttpMethod.Post))
        .InvalidModelState()
         .AndAlso()
         .ShouldReturn()
                 .View(view => view
                      .WithModelOfType<ProductFormModel>());




        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnRedirectToWithCorrectData()
            => MyController<ProductsController>
                .Instance(controller => controller
               .WithUser()
               .WithData(GetProduct()))
               .Calling(c => c.Delete(ProductTestId, new ProductDeleteFormModel
               {
                   SureToDelete = true
               }))
             .ShouldHave()
              .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
              .ValidModelState()
              .Data(data => data.WithSet<Product>(set => set.Any(x=>
                       x.Id== ProductTestId &&
                       x.IsDeleted ==true &&
                       x.IsPublic== false&&
                       x.DeletedOn.HasValue)))
              .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
              .AndAlso()
              .ShouldReturn()
              .Redirect(redirect => redirect
                    .To<ProductsController>(c => c
                    .All(With.Any<ProductsQueryModel>())));



        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnRedirectToAdminAreaWithCorrectData()
        => MyController<ProductsController>
             .Instance(controller => controller
            .WithUser(new[] { AdminConstants.AdministratorRoleName })
            .WithData(GetProduct(ProductTestId, false)))
            .Calling(c => c.Delete(ProductTestId, new ProductDeleteFormModel
            {
                SureToDelete = true
            }))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
           .ValidModelState()
           .Data(data => data.WithSet<Product>(set => set
                    .Should()
                    .BeEmpty()))
           .TempData(tempData => tempData
                    .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
                 .To<Areas.Admin.Controllers.ProductsController>(c => c
                       .Existing(With.Any<ProductsQueryModel>())));



        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotMerchant()
        => MyController<ProductsController>
             .Instance(controller => controller
            .WithUser())
            .Calling(c => c.Delete(
                   ProductTestId, 
                   With.Default<ProductDeleteFormModel>()))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))
          .AndAlso()
          .ShouldReturn()
          .BadRequest();

        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenProductDoesNotExists()
         => MyController<ProductsController>
          .Instance(controller => controller
         .WithUser()
         .WithData(GetMerchant()))
         .Calling(c => c.Delete(
                ProductTestId,
                With.Default<ProductDeleteFormModel>()))
       .ShouldHave()
        .ActionAttributes(attributes => attributes
           .RestrictingForAuthorizedRequests()
           .RestrictingForHttpMethod(HttpMethod.Post))
       .AndAlso()
       .ShouldReturn()
       .BadRequest();

        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnNotFoundWhenUserIsAdminAndProductDoesNotExists()
         => MyController<ProductsController>
          .Instance(controller => controller
         .WithUser(new[] { AdminConstants.AdministratorRoleName})
         .WithData())
         .Calling(c => c.Delete(
                ProductTestId,
                new ProductDeleteFormModel 
                {
                    SureToDelete=true
                }))
       .ShouldHave()
        .ActionAttributes(attributes => attributes
           .RestrictingForAuthorizedRequests()
           .RestrictingForHttpMethod(HttpMethod.Post))
       .AndAlso()
       .ShouldReturn()
       .NotFound();

        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnRedirectToDetailsWhenProductWhenNotSureToDelete()
        => MyController<ProductsController>
         .Instance(controller => controller
        .WithUser()
        .WithData(GetProduct()))
        .Calling(c => c.Delete(
               ProductTestId,
               With.Default<ProductDeleteFormModel>()))
      .ShouldHave()
       .ActionAttributes(attributes => attributes
          .RestrictingForAuthorizedRequests()
          .RestrictingForHttpMethod(HttpMethod.Post))
      .AndAlso()
      .ShouldReturn()
      .Redirect(redirect => redirect
                 .To<ProductsController>(c => c
                       .Details(ProductTestId, With.Any<ProductsDetailsQueryModel>())));

    }
}
