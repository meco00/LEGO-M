namespace LegoM.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using LegoM.Data;
    using LegoM.Models;
    using LegoM.Models.Home;
    using LegoM.Models.Products;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly LegoMDbContext data;

        public HomeController(LegoMDbContext data)
           => this.data = data;

        public IActionResult Index() 
        {


            var products = this.data.Products
               .OrderByDescending(x => x.PublishedOn)
               .Select(x => new ProductListingViewModel()
               {
                   Id = x.Id,
                   Title = x.Title,
                   Price = x.Price,
                   Condition = x.ProductCondition.ToString()
               })
               .Take(3)
               .ToList();


            return this.View(new IndexViewModel
            {
                TotalProducts = this.data.Products.Count(),
                TotalUsers=0,
                Products=products
            }) ;
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
