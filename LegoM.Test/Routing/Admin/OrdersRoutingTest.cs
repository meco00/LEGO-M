namespace LegoM.Test.Routing.Admin
{
    using LegoM.Areas.Admin.Models.Orders;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    using OrdersController = Areas.Admin.Controllers.OrdersController;


    public class OrdersRoutingTest
    {
        [Fact]
        public void PostDeleteShouldBeMapped()
          => MyRouting
              .Configuration()
               .ShouldMap(request => request
                  .WithPath($"/Admin/Orders/Delete/{1}")
                   .WithMethod(HttpMethod.Post))
               .To<OrdersController>(c => c
               .Delete(1, With.Any<OrderDeleteFormModel>()));
    }
}
