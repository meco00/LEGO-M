namespace LegoM.Controllers
{
    using AutoMapper;
    using LegoM.Models.Home;
    using LegoM.Services.Products;
    using LegoM.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly IProductsService products;

        public HomeController(IStatisticsService statistics, IProductsService products)
        {
            this.statistics = statistics;
            this.products = products;
        }

        public IActionResult Index() 
        {
            var products = this.products
                .Latest()
                .ToList();

            var totalStatistics = this.statistics.Total();


            return this.View(new IndexViewModel
            {
                TotalProducts = totalStatistics.TotalProducts,
                TotalUsers = totalStatistics.TotalUsers,
                TotalProductsSold = totalStatistics.TotalProductsSold,
                Products = products
            }); 
        }

       

        public IActionResult Error() => View();
    }
}
