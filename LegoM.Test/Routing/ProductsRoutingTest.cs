namespace LegoM.Test.Routing
{
    using LegoM.Controllers;
    using LegoM.Models.Products;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.Products;

    public class ProductsRoutingTest
    {
        [Fact]
        public void PostAddShouldBeMapped()
          => MyRouting
              .Configuration()
               .ShouldMap(request => request
                  .WithPath($"/Products/Add")
                   .WithMethod(HttpMethod.Post))
               .To<ProductsController>(c => c
               .Add(With.Any<ProductFormModel>()));

        [Fact]
        public void PostEditShoulBeMapped()
          => MyRouting
              .Configuration()
               .ShouldMap(request => request
                  .WithPath($"/Products/Edit/{ProductTestId}")
                   .WithMethod(HttpMethod.Post))
               .To<ProductsController>(c => c
               .Edit(ProductTestId, With.Any<ProductFormModel>()));

        [Fact]
        public void PostDeleteShoulBeMapped()
      => MyRouting
          .Configuration()
           .ShouldMap(request => request
              .WithPath($"/Products/Delete/{ProductTestId}")
               .WithMethod(HttpMethod.Post))
           .To<ProductsController>(c => c
           .Delete(ProductTestId, With.Any<ProductDeleteFormModel>()));
     
             
    }
}
