namespace LegoM.Test.Controllers.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Areas.Admin.Models.Orders;
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    using OrdersController = Areas.Admin.Controllers.OrdersController;
    using static Data.Orders;

    public class OrdersControllerTest
    {
        [Fact]
        public void PostDeleteShouldDeleteOrderAndReturnRedirectToUnAccomplished()
            =>MyController<OrdersController>
                  .Instance(controller => controller
                        .WithUser(new[] {AdminConstants.AdministratorRoleName })
                        .WithData(GetOrder()))
                 .Calling(c => c.Delete(1, new OrderDeleteFormModel
                 {
                     SureToDelete = true

                 }))
              .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                   .Data(data => data.WithSet<Order>(set =>
                   {
                       set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                       set.Should().BeEmpty();
                   }))
               .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(OrdersController.UnAccomplished));

        [Fact]
        public void PostDeleteShouldReturnRedirectToUnAccomplishedWhenNotSureToDeleteOrder()
            => MyController<OrdersController>
                  .Instance(controller => controller
                        .WithUser(new[] { AdminConstants.AdministratorRoleName })
                        .WithData(GetOrder()))
                 .Calling(c => c.Delete(1, new OrderDeleteFormModel
                 {
                     SureToDelete = false

                 }))
              .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(OrdersController.UnAccomplished));

        [Fact]
        public void PostDeleteShouldReturnNotFoundWhenOrderDoesNotExists()
          => MyController<OrdersController>
                .Instance(controller => controller
                      .WithUser(new[] { AdminConstants.AdministratorRoleName }))
               .Calling(c => c.Delete(1, new OrderDeleteFormModel
               {
                   SureToDelete = true

               }))
            .ShouldHave()
                 .ActionAttributes(attributes => attributes
                                .RestrictingForHttpMethod(HttpMethod.Post))
                 .ValidModelState()
              .AndAlso()
              .ShouldReturn()
              .NotFound();

    }
}
