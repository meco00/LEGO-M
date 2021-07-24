namespace LegoM.Services.Products
{

using System.Collections.Generic;

    public class ProductQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ProductsPerPage { get; init; }

        public int TotalProducts { get; init; }

        public IEnumerable<ProductServiceModel> Products { get; init; }
    }
}
