namespace LegoM.Test.Pipeline.Api
{
    using FluentAssertions;
    using LegoM.Controllers.Api;
    using LegoM.Models.Api.Products;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.Products;

    public class ProductsApiPipelineTest
    {
        [Fact]
        public void AllShouldReturnCorrectModel()
            => MyPipeline
                .Configuration()
                .ShouldMap(request=>request
                    .WithPath("/api/products")
                    .WithMethod(HttpMethod.Get))
                .To<ProductsApiController>(c => c.All(With.Default<AllProductsApiRequestModel>()))
                .Which(controller => controller.WithData(GetProducts(2)))
                
                  .ShouldReturn()
                  .ActionResult<ProductQueryModel>(result => result
                     .Passing(model =>
                     {
                         model.Products.Should().HaveCount(2);

                         model.TotalProducts.Should().Be(2);

                     }));



    }
}
