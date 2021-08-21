namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Areas.Admin.Models.Orders;
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Orders;

    using OrdersController = Areas.Admin.Controllers.OrdersController;

    public class OrdersPipelineTest
    {
        [Fact]
        public void UnAccomplishedShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                   .WithLocation("/Admin/Orders/UnAccomplished")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c
                       .UnAccomplished(With.Default<OrdersQueryModel>()))
                 .Which(controller => controller.WithData(GetOrders(5)))
                 .ShouldReturn()
                 .View(view => view
                    .WithModelOfType<OrdersQueryModel>()
                    .Passing(model => 
                    {
                        model.Orders.Should().HaveCount(5);
                        model.TotalOrders.Should().Be(5);
                     }));


        [Fact]
        public void AccomplishedShouldReturnViewWithCorrectModelAndData()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                   .WithLocation("/Admin/Orders/Accomplished")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c
                       .Accomplished(With.Default<OrdersQueryModel>()))
                 .Which(controller => controller.WithData(GetOrders(5,accomplished:true)))
                 .ShouldReturn()
                 .View(view => view
                    .WithModelOfType<OrdersQueryModel>()
                    .Passing(model =>
                    {
                        model.Orders.Should().HaveCount(5);
                        model.TotalOrders.Should().Be(5);
                    }));


        [Theory]
        [InlineData(1,1,5)]
        public void AccomplishShouldChangeOrderIfIsUnAccomplishedAndReturnRedirectToAccomplished(
            int cartItemsCount,
            byte quantityPerCartItem,
            byte quantityPerProduct)

          => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                   .WithLocation($"/Admin/Orders/Accomplish/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c => c
                       .Accomplish(1))
                 .Which(controller => controller
                         .WithData(GetOrder(
                          cartItemsCount:cartItemsCount,
                          quantityPerItem:quantityPerCartItem,
                          quantityPerProduct:quantityPerProduct)))
                 .ShouldHave()
                 .TempData(tempData=>tempData
                        .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .Data(data => data.WithSet<Order>(set =>
                  {
                     var order = set.FirstOrDefault();
                     order.IsAccomplished.Should().BeTrue();
                     order.ShoppingCart.All(x => x.Quantity==quantityPerProduct-quantityPerCartItem);
                     order.ShoppingCart.All(x => x.UserId==null);

                  }))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect=>redirect
                      .To<OrdersController>(c=>c
                      .Accomplished(With.Any<OrdersQueryModel>())));


        [Fact]
        public void AccomplishShouldReturnNotFoundWhenOrderDoesNotExists()
        => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                 .WithLocation($"/Admin/Orders/Accomplish/{1}")
                 .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .To<OrdersController>(c => c
                     .Accomplish(1))
               .Which()
               .ShouldReturn()
               .NotFound();


        [Theory]
        [InlineData(1, 1, 5)]
        public void CancelShouldRetunRedirectToUnAccomplished(
            int cartItemsCount,
            byte quantityPerCartItem,
            byte quantityPerProduct)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"Admin/Orders/Cancel/{1}")
                     .WithUser(new[] {AdminConstants.AdministratorRoleName }))
                 .To<OrdersController>(c=>c.Cancel(1))
                  .Which(controller => controller
                         .WithData(GetOrder(
                          accomplished:true,
                          cartItemsCount: cartItemsCount,
                          quantityPerItem: quantityPerCartItem,
                          quantityPerProduct: quantityPerProduct)))
                 .ShouldHave()
                 .TempData(tempData => tempData
                        .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .Data(data => data.WithSet<Order>(set =>
                  {
                      var order = set.FirstOrDefault();
                      order.IsAccomplished.Should().BeFalse();
                      order.ShoppingCart.All(x => x.Product.Quantity == quantityPerProduct + quantityPerCartItem);
                      
                  }))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<OrdersController>(c => c
                      .UnAccomplished(With.Any<OrdersQueryModel>())));

        [Fact]
        public void CancelShouldReturnNotFoundWhenOrderDoesNotExists()
       => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                .WithLocation($"/Admin/Orders/Cancel/{1}")
                .WithUser(new[] { AdminConstants.AdministratorRoleName }))
              .To<OrdersController>(c => c
                    .Cancel(1))
              .Which()
              .ShouldReturn()
              .NotFound();


        [Fact]
        public void GetDeleteShoulReturnViewWithCorrectModel()
            => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                .WithLocation($"/Admin/Orders/Delete/{1}")
                .WithUser(new[] { AdminConstants.AdministratorRoleName }))
              .To<OrdersController>(c => c
                    .Delete(1))
              .Which(controller=>controller
                  .WithData(GetOrder())
              .ShouldReturn()
              .View());


        [Fact]
        public void GetDeleteShoulReturnNotFoundWhenOrderDoesNotExists()
           => MyPipeline
             .Configuration()
             .ShouldMap(request => request
               .WithLocation($"/Admin/Orders/Delete/{1}")
               .WithUser(new[] { AdminConstants.AdministratorRoleName }))
             .To<OrdersController>(c => c
                   .Delete(1))
             .Which()
             .ShouldReturn()
             .NotFound();


    }
}
