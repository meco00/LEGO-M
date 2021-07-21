namespace LegoM.Models.Products
{
    using LegoM.Data.Models.Enums;
    using System.Collections.Generic;

    public class ProductsQueryModel
    {
        public IEnumerable<string> AllCategories { get; init; }

        public IEnumerable<string> AllSubCategories { get; init; }

        public string SearchTerm { get; init; }

        public ProductSorting productSorting { get; init; }

       public IEnumerable<ProductListingViewModel> Products { get; init; }
    }
}
