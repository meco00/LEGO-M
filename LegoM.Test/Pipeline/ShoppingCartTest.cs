namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.ShoppingCarts.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.ShoppingCartItems;
    using static Data.Products;
    using static Data.DataConstants;
    using LegoM.Models.Products;

    public class ShoppingCartTest
    {
        [Fact]
        public void MineShouldBeForAuthorizedUsersAndReturnViewWithCorrectDataAndModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithLocation("/ShoppingCart/Mine")
                 .WithUser())
                 .To<ShoppingCartController>(c => c.Mine())
                 .Which(controller => controller.WithData(GetShoppingCartItems()))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                             .RestrictingForAuthorizedRequests())
                  .AndAlso()
                  .ShouldReturn()
                   .View(view => view.WithModelOfType<List<ShoppingCartItemServiceModel>>()
                     .Passing(model=>model.Should().HaveCount(5)));


        [Fact]
        public void DeleteShouldBeForAuthorizedUsersAndDeleteCartItemAndReturnRedirectToMine()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request.WithLocation($"/ShoppingCart/Delete/{1}")
                  .WithUser())
                  .To<ShoppingCartController>(c => c.Delete(1))
                  .Which(controller => controller.WithData(GetShoppingCartItems(1)))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                             .RestrictingForAuthorizedRequests())
                  .Data(data => data.WithSet<ShoppingCartItem>(set =>
                  {
                      set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                      set.Should().BeEmpty();
                  }))
                  .AndAlso()
                  .ShouldReturn()
                  .Redirect(redirect => redirect
                    .To<ShoppingCartController>(c => c.Mine()));

        [Fact]
        public void DeleteShouldBeForAuthorizedUsersAndReturnBadRequestWhenCartItemIsNotOfUser()
          => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithLocation($"/ShoppingCart/Delete/{1}")
                .WithUser())
                .To<ShoppingCartController>(c => c.Delete(1))
                .Which(controller => controller.WithData(GetShoppingCartItems(1, sameUser: false)))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                           .RestrictingForAuthorizedRequests())    
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void DeleteShouldBeForAuthorizedUsersAndReturnNotFoundUserIsAdminAndCartItemDoesNotExists()
         => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithLocation($"/ShoppingCart/Delete/{1}")
               .WithUser(new[] { AdminConstants.AdministratorRoleName}))
               .To<ShoppingCartController>(c => c.Delete(1))
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .NotFound();

        [Fact]
        public void AddShouldBeForAuthorizedUsersAndReturnRedirectToMineWIthCorrectDataAndModel()
            => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithLocation($"/ShoppingCart/Add/{TestId}")
                .WithUser())
                .To<ShoppingCartController>(c => c.Add(TestId))
                 .Which(controller => controller.WithData(GetProduct()))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                 .Data(data => data.WithSet<ShoppingCartItem>(set =>
                 {
                     var cartItem = set.FirstOrDefault();

                     cartItem.ProductId.Should().Be(TestId);
                     cartItem.UserId.Should().Be(TestUser.Identifier);
                     cartItem.Quantity.Should().Be(1);

                 }))
                 .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                       .AndAlso()
                       .ShouldReturn()
                       .Redirect(redirect => redirect
                          .To<ProductsController>(c => c
                          .Details(TestId,With.Any<ProductsDetailsQueryModel>())));


        [Fact]
        public void AddShouldBeForAuthorizedUsersAndReturnNotFoundWhenProductDoesNotExists()
           => MyPipeline
               .Configuration()
                .ShouldMap(request => request.WithLocation($"/ShoppingCart/Add/{TestId}")
               .WithUser())
               .To<ShoppingCartController>(c => c.Add(TestId))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                  .AndAlso()
                  .ShouldReturn()
                     .NotFound();


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithLocation($"/ShoppingCart/Edit/{1}")
               .WithUser())
               .To<ShoppingCartController>(c => c.Edit(1))
                .Which(controller => controller.WithData(GetShoppingCartItems(1)))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view
                     .WithModelOfType<CartItemServiceModel>()
                   .Passing(model =>
                   {
                       model.Quantity.Should().Be(1);
                       model.ProductQuantity.Should().Be(5);

                   }));


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenCartItemIsNotOfUser()
          => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithLocation($"/ShoppingCart/Edit/{1}")
             .WithUser())
             .To<ShoppingCartController>(c => c.Edit(1))
              .Which(controller => controller.WithData(GetShoppingCartItems(1, sameUser: false)))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())      
                 .AndAlso()
                 .ShouldReturn()
                 .BadRequest();


      



    }
}
