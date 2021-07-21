namespace LegoM.Models.Products
{
    using LegoM.Data.Models.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductsQueryModel
    {
        public string Category { get; set; }

        public IEnumerable<string> Categories { get; init; }

        public IEnumerable<string> AllSubCategories { get; init; }

        [Display(Name ="Deep Search")]
        public string SearchTerm { get; init; }

        public ProductSorting ProductSorting { get; init; }

       public IEnumerable<ProductListingViewModel> Products { get; init; }
    }
}
