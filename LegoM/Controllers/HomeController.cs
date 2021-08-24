namespace LegoM.Controllers
{
    using LegoM.Services.Products;
    using LegoM.Services.Products.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static WebConstants.Cache;

    public class HomeController : Controller
    {
        private readonly IProductsService products;
        private readonly IMemoryCache cache;

        public HomeController( IProductsService products,IMemoryCache cache)
        {   
            this.products = products;
            this.cache = cache;
        }

        public IActionResult Index() 
        {  
            var latestProducts = this.cache.Get<List<ProductServiceModel>>(LatestProductsCacheKey);

            if (latestProducts == null)
            {
            var products = this.products
                .Latest()
                .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                latestProducts = this.cache.Set(LatestProductsCacheKey, products,cacheOptions);
            }     

            return this.View(latestProducts); 
        }

        public IActionResult Error() => View();
    }
}
