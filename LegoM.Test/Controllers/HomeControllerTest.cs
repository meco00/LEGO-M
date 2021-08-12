namespace LegoM.Test.Controllers
{
    using FluentAssertions;
    using LegoM.Controllers;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    using static WebConstants.Cache;
    using static Data.Products;
    using System;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnCorrectModelAndView()
        => MyController<HomeController>
                  .Instance(controller => controller
                  .WithData(TenPublicProducts()))
            .Calling(c => c.Index())
            .ShouldHave()
            .MemoryCache(cache=>cache
            .ContainingEntry(entry=>entry
                  .WithKey(LatestProductsCacheKey)
                  .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                  .WithValueOfType<List<ProductServiceModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view.WithModelOfType<List<ProductServiceModel>>()
            .Passing(model => model.Should().HaveCount(3)));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(x => x.Error())
            .ShouldReturn()
            .View();
                  
    }
}
