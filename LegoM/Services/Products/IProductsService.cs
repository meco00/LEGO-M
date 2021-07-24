
namespace LegoM.Services.Products
{
  using LegoM.Data.Models.Enums;
    using System.Collections.Generic;

    public interface IProductsService
    {
        ProductQueryServiceModel All(
            string category,
            string searchTerm,
            int currentPage,
            int productsPerPage,
            ProductSorting productSorting);

        IEnumerable<string> AllProductCategories();
    }
}
