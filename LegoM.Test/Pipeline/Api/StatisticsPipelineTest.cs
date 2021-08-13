namespace LegoM.Test.Pipeline.Api
{
    using FluentAssertions;
    using LegoM.Controllers.Api;
    using LegoM.Services.Statistics.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Products;
    using static Data.Users;

    public class StatisticsPipelineTest
    {
        [Fact]
        public void TotalShouldReturnCorrectModel()
            => MyPipeline
                .Configuration()
                 .ShouldMap(request=>request
                    .WithPath("/api/statistics")
                    .WithMethod(HttpMethod.Get))
                 .To<StatisticsApiController>(c => c.GetStatistics())
                 .Which(controller => controller
                       .WithData(GetUsers())
                       .AndAlso()
                       .WithData(GetPublicProducts())                       
                  .ShouldReturn()
                  .ResultOfType<StatisticsServiceModel>(result=>result
                        .Passing(model =>
                        {
                            model.TotalProducts.Should().NotBe(0);

                            model.TotalUsers.Should().NotBe(0);

                         })));


    }
}
