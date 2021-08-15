namespace LegoM.Test.Routing
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
    }
}
