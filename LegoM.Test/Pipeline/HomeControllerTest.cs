
using LegoM.Test.Data;

namespace LegoM.Test.Pipeline
{
    using AutoMapper;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.Products;
    using LegoM.Services.Statistics;
    using LegoM.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using Xunit;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;

    using static Products;

    using static WebConstants.Cache;
    using System;

    public class HomeControllerTest
    {

        [Fact]
        public void IndexShouldReturnViewWithCorrectDataAndModel()
        => MyPipeline
            .Configuration()
            .ShouldMap(request=>request.WithLocation("/")
            .WithUser()
            .WithAntiForgeryToken()
            )
            .To<HomeController>(c => c.Index())
            .Which(controller => controller                      
                  .WithData(TenPublicProducts()))            
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<ProductServiceModel>>()
                .Passing(m => m.Should().HaveCount(3)));



        [Fact]
        public void ErrorShouldReturnView()
         => MyMvc.
            Pipeline()
            .ShouldMap("/Home/Error")
            .To<HomeController>(c => c.Error())
            .Which()
            .ShouldReturn()
            .View();

      

    }
}
