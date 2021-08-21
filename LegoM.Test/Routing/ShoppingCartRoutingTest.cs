namespace LegoM.Test.Routing
{
    using LegoM.Controllers;
    using LegoM.Services.ShoppingCarts.Models;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class ShoppingCartRoutingTest
    {
        [Fact]
        public void PostEditShouldBeMapped()
           => MyRouting
               .Configuration()
                .ShouldMap(request => request
                   .WithPath($"/ShoppingCart/Edit/{1}")
                    .WithMethod(HttpMethod.Post))
                .To<ShoppingCartController>(c => c
                .Edit(1, With.Any<CartItemServiceModel>()));
    }
}
