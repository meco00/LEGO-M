
namespace LegoM.Services.Products
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;

    public interface IProductsService
    {
        ProductQueryServiceModel All(
            string category,
            string searchTerm,
            int currentPage,
            int productsPerPage,
            ProductSorting productSorting);

        IEnumerable<ProductServiceModel> Latest();

        string Create(string title,
                string description,
                decimal price,
                byte quantity,
                string categoryId,
                string subCategoryId,
                ProductCondition productCondition,
                DeliveryTake productDelivery,
                string merchantId
                );

        bool Edit(
               string Id,
               string title,
               string description,
               decimal price,
               byte quantity,
               string categoryId,
               string subCategoryId,
               ProductCondition productCondition,
               DeliveryTake productDelivery,
               string merchantId
               );

        ProductDetailsServiceModel Details(string Id);

        IEnumerable<ProductServiceModel> ByUser(string userId);

        bool ProductIsByMerchant(string id, string merchantId);

        IEnumerable<string> Categories();

        IEnumerable<ProductCategoryServiceModel> AllCategories();

        IEnumerable<ProductSubCategoryServiceModel> AllSubCategories();

        bool CategoryExists(string categoryId);

        bool SubCategoryExists(string subCategoryId,string categoryId);


        
            
    }
}
