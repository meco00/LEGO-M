namespace LegoM.Models.Products
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductsQueryModel
    {
        public const int ProductsPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string Category { get; init; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<string> AllSubCategories { get; set; }

        [Display(Name ="Deep Search")]
        public string SearchTerm { get; init; }

        public ProductSorting ProductSorting { get; init; }

        public int TotalProducts { get; set; }

       public IEnumerable<ProductServiceModel> Products { get; set; }
    }
}
