namespace LegoM.Services.Statistics
{
    using LegoM.Data;
    using LegoM.Services.Statistics.Models;
    using System.Linq;

    public class StatisticsService : IStatisticsService
    {
        private readonly LegoMDbContext data;

        public StatisticsService(LegoMDbContext data)
        => this.data = data;


        public StatisticsServiceModel Total()
        {
            var totalProducts = this.data.Products.Count(c=>c.IsPublic);
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalProducts = totalProducts,
                TotalUsers = totalUsers,

            };
        }
    }
}

