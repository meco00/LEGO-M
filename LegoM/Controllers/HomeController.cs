namespace LegoM.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using LegoM.Data;
    using LegoM.Models;
    using LegoM.Models.Home;
    using LegoM.Services.Products;
    using LegoM.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly LegoMDbContext data;

        private readonly IStatisticsService statistics;

        public HomeController(LegoMDbContext data, IStatisticsService statistics)
        {
            this.data = data;
            this.statistics = statistics;
        }

        public IActionResult Index() 
        {


            var products = this.data.Products
               .OrderByDescending(x => x.PublishedOn)
               .Select(x => new ProductServiceModel()
               {
                   Id = x.Id,
                   Title = x.Title,
                   Price = x.Price,
                   Condition = x.ProductCondition.ToString()
               })
               .Take(3)
               .ToList();

            var totalStatistics = this.statistics.Total();


            return this.View(new IndexViewModel
            {
                TotalProducts = totalStatistics.TotalProducts,
                TotalUsers=totalStatistics.TotalUsers,
                TotalProductsSold=totalStatistics.TotalProductsSold,
                Products=products
            }) ;
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
