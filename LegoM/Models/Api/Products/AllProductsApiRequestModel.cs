namespace LegoM.Models.Api.Products
{
    using LegoM.Data.Models.Enums;


    public class AllProductsApiRequestModel
    {
        public int CurrentPage { get; set; } = 1;

        public string Category { get; init; }

        public string SubCategory { get; init; }

        public string SearchTerm { get; init; }

        public int ProductsPerPage { get; set; } = 10;

        public ProductSorting ProductSorting { get; init; }

      
    }
}
