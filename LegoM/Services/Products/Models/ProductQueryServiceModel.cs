namespace LegoM.Services.Products.Models
{
  using System.Collections.Generic;

    public class ProductQueryModel
    {
        public int CurrentPage { get; init; }

        public int ProductsPerPage { get; init; }

        public int TotalProducts { get; init; }        

        public IEnumerable<ProductServiceModel> Products { get; init; }
    }
}
