namespace LegoM.Test.Controllers
{
    using LegoM.Controllers;
    using LegoM.Models.Products;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using LegoM.Data.Models.Enums;
    using LegoM.Data.Models;
    using FluentAssertions;

    using static Data.Users;
    using static Data.Categories;
    using static Data.Products;
    using static Data.DataConstants;
    using LegoM.Areas.Admin;

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
                       .Details(With.Any<string>())));


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


        [Fact]
        public void PostEditShouldEditProductAndReturnRedirectToWithCorrectDataAndModel()
            => MyController<ProductsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetProduct()))
                 .Calling(c => c.Edit(TestId,new ProductFormModel
                 {
                     Title = "TitleTest",
                     Description = "DescriptionTest",
                     Price = 12.50M,
                     Quantity = 2,
                     FirstImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Cat_img.jpg",
                     CategoryId = 2,
                     SubCategoryId = 1,
                     Condition = ProductCondition.New,
                     Delivery = DeliveryTake.Seller,

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
                             x.CategoryId == 2 &&
                             x.SubCategoryId == 1 &&
                             x.IsPublic == false &&
                             x.ProductCondition == ProductCondition.New &&
                             x.DeliveryTake == DeliveryTake.Seller)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                       .To<ProductsController>(c => c
                       .Details(TestId)));



        [Fact]
        public void PostEditShouldReturnBadRequestWhenUserIsNotMerchant()
         => MyController<ProductsController>
             .Instance(controller => controller
                     .WithUser())
              .Calling(c => c.Edit(TestId,new ProductFormModel
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
                  .WithUser()
                  .WithData(GetMerchant()))
           .Calling(c => c.Edit(TestId, With.Any<ProductFormModel>()))
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
                   .WithData(GetProduct(TestId,false)))
            .Calling(c => c.Edit(TestId, new ProductFormModel
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
        .Calling(c => c.Edit(TestId, new ProductFormModel
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
               .Calling(c => c.Delete(TestId, new ProductDeleteFormModel
               {
                   SureToDelete = true
               }))
             .ShouldHave()
              .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests()
                 .RestrictingForHttpMethod(HttpMethod.Post))
              .ValidModelState()
              .Data(data => data.WithSet<Product>(set => set.Any(x=>
                       x.Id==TestId &&
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
            .WithData(GetProduct(TestId,false)))
            .Calling(c => c.Delete(TestId, new ProductDeleteFormModel
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
                       .Existing(With.Default<ProductsQueryModel>())));



        [Fact]
        public void PostDeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenUserIsNotMerchant()
        => MyController<ProductsController>
             .Instance(controller => controller
            .WithUser())
            .Calling(c => c.Delete(
                   TestId, 
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
                TestId,
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
                TestId,
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
               TestId,
               With.Default<ProductDeleteFormModel>()))
      .ShouldHave()
       .ActionAttributes(attributes => attributes
          .RestrictingForAuthorizedRequests()
          .RestrictingForHttpMethod(HttpMethod.Post))
      .AndAlso()
      .ShouldReturn()
      .Redirect(redirect => redirect
                 .To<ProductsController>(c => c
                       .Details(TestId)));







    }
}
