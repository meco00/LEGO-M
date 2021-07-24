using LegoM.Data;
using System.Linq;

namespace LegoM.Services.Statistics
{


    public class StatisticsService : IStatisticsService
    {
        private readonly LegoMDbContext data;

        public StatisticsService(LegoMDbContext data)
        => this.data = data;


        public StatisticsServiceModel Total()
        {
            var totalProducts = this.data.Products.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalProducts = totalProducts,
                TotalUsers = totalUsers,
                TotalProductsSold = 0

            };
        }
    }
}

