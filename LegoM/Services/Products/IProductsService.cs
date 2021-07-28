
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

        string Create(string title,
                string description,
                decimal price,
                byte quantity,
                ProductCondition productCondition,
                DeliveryTake productDelivery,
                string merchantId,
                IEnumerable<string> subCategoriesIds);

        bool Edit(
               string Id,
               string title,
               string description,
               decimal price,
               byte quantity,
               ProductCondition productCondition,
               DeliveryTake productDelivery,
               string merchantId,
               IEnumerable<string> subCategoriesIds);

        ProductDetailsServiceModel Details(string Id);

        IEnumerable<ProductServiceModel> ByUser(string userId);

        bool ProductIsByMerchant(string id, string merchantId);

        IEnumerable<string> AllCategories();

        IEnumerable<ProductSubCategoryServiceModel> AllSubCategories();

        bool SubCategoriesExists(IEnumerable<string> subCategoriesIds);

        
            
    }
}
