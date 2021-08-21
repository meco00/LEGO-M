namespace LegoM.Test.Controllers
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.ShoppingCarts.Models;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.ShoppingCartItems;

    public class ShoppingCartControllerTest
    {
        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnRedirectToMineWithCorrectData()
          => MyController<ShoppingCartController>
             .Instance(instance=>instance
             .WithUser()
             .WithData(GetShoppingCartItems(1)))
             .Calling(c => c.Edit(1, new CartItemServiceModel
             {
                 Quantity = 3,
                 ProductQuantity = 5
             }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests()
                      .RestrictingForHttpMethod(HttpMethod.Post))
             .Data(data => data.WithSet<ShoppingCartItem>(set =>
             {
                 var cartItem = set.Find(1);

                 cartItem.Quantity.Should().Be(3);


             }))
              .TempData(tempData => tempData
                     .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
             .AndAlso()
             .ShouldReturn()
             .Redirect(redirect => redirect
                  .To<ShoppingCartController>(c => c.Mine()));


        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenCartItemIsNotOfUser()
         => MyController<ShoppingCartController>
            .Instance(instance => instance
            .WithUser()
            .WithData(GetShoppingCartItems(1, sameUser: false)))
            .Calling(c => c.Edit(1, new CartItemServiceModel
            {
                Quantity = 3,
                ProductQuantity = 5
            }))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests()
                     .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .BadRequest();


        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnNotFoundWhenUserIsAdminCartItemDoesNotExists()
        => MyController<ShoppingCartController>
           .Instance(instance => instance
           .WithUser(new[] { AdminConstants.AdministratorRoleName}))
           .Calling(c => c.Edit(1, new CartItemServiceModel
           {
               Quantity = 3,
               ProductQuantity = 5
           }))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
           .AndAlso()
           .ShouldReturn()
           .NotFound();

    }
}
