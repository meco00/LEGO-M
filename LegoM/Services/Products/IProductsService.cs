
namespace LegoM.Services.Products
{
  using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using System.Collections.Generic;

    public interface IProductsService
    {
        ProductQueryServiceModel All(
            string category,
            string searchTerm,
            int currentPage,
            int productsPerPage,
            ProductSorting productSorting);

        ProductDetailsServiceModel Details(string Id);

        IEnumerable<ProductServiceModel> ByUser(string userId);

        IEnumerable<string> AllCategories();

        IEnumerable<ProductSubCategoryServiceModel> AllSubCategories();

        bool SubCategoriesExists(IEnumerable<string> subCategoriesIds);

        
            
    }
}
