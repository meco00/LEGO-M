namespace LegoM.Test.Controllers.Api
{
    using LegoM.Controllers.Api;
    using LegoM.Test.Mocks;
    using Xunit;

    public class StatisticsApiControllerTest
    {
        [Fact]
        public void GetStatisticsShouldReturnTotalStatistics()
        {
            //Arange
            var statisticsController = new StatisticsApiController(StatisticsServiceMock.Instance);

            //Act
            var result = statisticsController.GetStatistics();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.TotalProducts);
            Assert.Equal(2, result.TotalUsers);
            Assert.Equal(1, result.TotalProductsSold);


        }

    }
}
