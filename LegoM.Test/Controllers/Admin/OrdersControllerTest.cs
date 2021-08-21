namespace LegoM.Test.Controllers.Admin
{
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using OrdersController = Areas.Admin.Controllers.OrdersController;

    using static Data.Orders;
    using LegoM.Areas.Admin.Models.Orders;
    using LegoM.Data.Models;
    using FluentAssertions;

    public class OrdersControllerTest
    {
        [Fact]
        public void PostDeleteShouldDeleteProductAndReturnRedirectToUnAccomplished()
            =>MyController<OrdersController>
                  .Instance(controller => controller
                        .WithUser()
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
                .Redirect(redirect => redirect
                       .To<Areas.Admin.Controllers.OrdersController>(c => c
                       .UnAccomplished(With.Any<OrdersQueryModel>())));
    }
}
