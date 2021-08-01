using LegoM.Services.Statistics;
using Moq;

namespace LegoM.Test.Mocks
{

   public static class StatisticsServiceMock
    {

        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(x => x.Total())
                    .Returns(new StatisticsServiceModel
                    {
                        TotalProducts=5,
                        TotalUsers=2,
                        TotalProductsSold=1
                    });

                return statisticsServiceMock.Object;
            }
        }
    }
}
