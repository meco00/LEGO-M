namespace LegoM.Test.Pipeline.Api
{
    using FluentAssertions;
    using LegoM.Controllers.Api;
    using LegoM.Services.Statistics.Models;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using static Data.Products;
    using static Data.Users;

    public class StatisticsPipelineTest
    {
        [Theory]
        [InlineData(5,5)]
        public void TotalShouldReturnCorrectModel(
            int usersCount,
            int productsCount)
            => MyPipeline
                .Configuration()
                 .ShouldMap(request=>request
                    .WithPath("/api/statistics")
                    .WithMethod(HttpMethod.Get))
                 .To<StatisticsApiController>(c => c.GetStatistics())
                 .Which(controller => controller
                       .WithData(GetUsers())
                       .AndAlso()
                       .WithData(GetProducts())                       
                  .ShouldReturn()
                  .ResultOfType<StatisticsServiceModel>(result=>result
                        .Passing(model =>
                        {
                            model.TotalProducts.Should().Be(productsCount);

                            model.TotalUsers.Should().Be(usersCount);

                         })));


    }
}
