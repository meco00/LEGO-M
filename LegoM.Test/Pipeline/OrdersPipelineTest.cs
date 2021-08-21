namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.Orders.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.ShoppingCartItems;
    using static Data.Users;

    public class OrdersPipelineTest
    {
        [Fact]
        public void GetAddOrderShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                  .WithLocation("/Orders/Add")
                  .WithUser())
                  
               .To<OrdersController>(c => c.Add())
            .Which(controller => controller
                 .WithData(GetShoppingCartItems())
                 .AndAlso()
                 .WithData(GetMerchant())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<OrderFormServiceModel>()
                 .Passing(model =>
                 {
                     model.FullName.Should().BeNull();
                     model.TelephoneNumber.Should().Be(TelephoneNumber);

                 })));


        [Fact]
        public void GetAddOrderShouldBeForAuthorizedUsersAndReturnRedirectToShoppingCartMineWhenCartContainsInvalidItems()
    => MyPipeline
        .Configuration()
        .ShouldMap(request => request
          .WithLocation("/Orders/Add")
          .WithUser())
      
          .To<OrdersController>(c => c.Add())
         .Which(controller => controller
            .WithData(GetShoppingCartItems(cartQuantity: 0, productQuantity: 0)))
          .ShouldHave()
          .ActionAttributes(attributes => attributes
            .RestrictingForAuthorizedRequests())
           .TempData(tempData => tempData
               .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
           .AndAlso()
           .ShouldReturn()
           .Redirect(redirect => redirect
            .To<ShoppingCartController>(c => c
            .Mine()));



        [Theory]
        [InlineData("Ivan Ivanov",TelephoneNumber, "Kirdjali", "Momchilgrad", "Bacho Kiro 15", "6800",3)]
        public void PostAddOrderShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel(
            string fullName,
            string telephoneNumber,
            string state,
            string city,
            string address,
            string zipCode,
            int cartItemsCount)
          => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                .WithLocation("/Orders/Add")
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new
                {
                    FullName = fullName,
                    TelephoneNumber = telephoneNumber,
                    State = state,
                    City = city,
                    Address = address,
                    ZipCode = zipCode

                })
                .WithUser()
                .WithAntiForgeryToken())

             .To<OrdersController>(c => c.Add(new OrderFormServiceModel
             {
                 FullName = fullName,
                 TelephoneNumber = TelephoneNumber,
                 State = state,
                 City = city,
                 Address = address,
                 ZipCode = zipCode

             }))
          .Which(controller => controller
               .WithData(GetShoppingCartItems(cartItemsCount)))
          .ShouldHave()
          .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
            .ValidModelState()
           .Data(data => data.WithSet<Order>(set => set.Any(x =>
                   x.FullName == fullName &&
                   x.PhoneNumber == telephoneNumber &&
                   x.State == state &&
                   x.City == city &&
                   x.Address == address &&
                   x.ZipCode == zipCode &&
                   x.ShoppingCart.Count == cartItemsCount)))
            .TempData(tempData => tempData
                .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
          .AndAlso()
          .ShouldReturn()
          .Redirect(redirect => redirect
                .To<HomeController>(c =>
                c.Index()));





        [Theory]
        [InlineData("Ivan Ivanov", TelephoneNumber, "Kirdjali", "Momchilgrad", "Bacho Kiro 15", "6800", 3)]
        public void PostAddOrderShouldBeForAuthorizedUsersAndReturnRedirectToShoppingCartMineWhenCartHasInvalidItems(
           string fullName,
           string telephoneNumber,
           string state,
           string city,
           string address,
           string zipCode,
           int cartItemsCount)
         => MyPipeline
             .Configuration()
             .ShouldMap(request => request
               .WithLocation("/Orders/Add")
               .WithMethod(HttpMethod.Post)
               .WithFormFields(new
               {
                   FullName = fullName,
                   TelephoneNumber = telephoneNumber,
                   State = state,
                   City = city,
                   Address = address,
                   ZipCode = zipCode

               })
               .WithUser()
               .WithAntiForgeryToken())

            .To<OrdersController>(c => c.Add(new OrderFormServiceModel
            {
                FullName = fullName,
                TelephoneNumber = TelephoneNumber,
                State = state,
                City = city,
                Address = address,
                ZipCode = zipCode

            }))
         .Which(controller => controller
              .WithData(GetShoppingCartItems(cartItemsCount,cartQuantity:0,productQuantity:0)))
         .ShouldHave()
         .ActionAttributes(attributes => attributes
              .RestrictingForAuthorizedRequests()
              .RestrictingForHttpMethod(HttpMethod.Post))           
           .TempData(tempData => tempData
               .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
         .AndAlso()
         .ShouldReturn()
         .Redirect(redirect => redirect
               .To<ShoppingCartController>(c =>
               c.Mine()));


    }
}
