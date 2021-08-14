﻿namespace LegoM.Test.Pipeline.Api
{
    using FluentAssertions;
    using LegoM.Controllers.Api;
    using LegoM.Models.Api.Products;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
                .Which(controller => controller.WithData(GetPublicProducts(2)))
                
                  .ShouldReturn()
                  .ActionResult<ProductQueryServiceModel>(result => result
                     .Passing(model =>
                     {
                         model.Products.Should().HaveCount(2);

                         model.TotalProducts.Should().Be(2);

                     }));



    }
}