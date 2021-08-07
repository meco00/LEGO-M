namespace LegoM.Services.Products.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductDetailsAndSimilarProductsServiceModel
    {
        public ProductDetailsServiceModel  Product { get; init; }

        public IEnumerable<ProductServiceModel> SimilarProducts { get; init; }
    }
}
