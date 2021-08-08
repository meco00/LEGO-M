
namespace LegoM.Services.Products
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;

    public interface IProductsService
    {
        ProductQueryServiceModel All(
            string category,
            string subCategory,
            string searchTerm,
            int currentPage,
            int productsPerPage,
            ProductSorting productSorting);

        IEnumerable<ProductServiceModel> Latest();

        string Create(string title,
                string description,
                string firstImageUrl,
                string secondImageUrl,
                string thirdImageUrl,
                decimal price,
                byte quantity,
                int categoryId,
                int subCategoryId,
                ProductCondition productCondition,
                DeliveryTake productDelivery,
                string merchantId
                );

        bool Edit(
               string Id,
               string title,
               string description,
               string firstImageUrl,
               string secondImageUrl,
               string thirdImageUrl,
               decimal price,
               byte quantity,
               int categoryId,
               int subCategoryId,
               ProductCondition productCondition,
               DeliveryTake productDelivery,
               string merchantId
               );

        ProductDetailsServiceModel Details(string Id);

        IEnumerable<ProductServiceModel> ByUser(string userId);



        IEnumerable<ProductServiceModel> GetSimilarProducts(string Id);

        bool ProductIsByMerchant(string id, string merchantId);

        IEnumerable<string> Categories();

        IEnumerable<ProductCategoryServiceModel> AllCategories();

        IEnumerable<ProductSubCategoryServiceModel> AllSubCategories();

        bool ProductExists(string Id);

        bool CategoryExists(int categoryId);

        bool SubCategoryExists(int subCategoryId, int categoryId);

        bool SubCategoryParticipateInCategory(string subCategory,string category);

        bool SubCategoryIsValid(string subCategory,string category);





    }
}
