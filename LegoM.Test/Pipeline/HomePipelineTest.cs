﻿
using LegoM.Test.Data;

namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Controllers;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;
    using static Products;

    public class HomePipelineTest
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
                  .WithData(GetPublicProducts()))            
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