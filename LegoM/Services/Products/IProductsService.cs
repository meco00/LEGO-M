
namespace LegoM.Services.Products
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;

    public interface IProductsService
    {
        ProductQueryModel All(
            string category=null,
            string subCategory=null,
            string searchTerm=null,
            int currentPage = 1,
            int productsPerPage = int.MaxValue,
            ProductSorting productSorting = ProductSorting.Newest,
            bool IsPublicOnly = true,
            bool IsDeleted=false);

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
                string merchantId,
                bool IsPublic = false
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
               string merchantId,
               bool IsPublic = false
               );

        ProductDetailsServiceModel Details(string Id);

        IEnumerable<ProductServiceModel> ByUser(string userId);

        IEnumerable<ProductServiceModel> GetSimilarProducts(string Id);

        bool ProductIsByTrader(string id, string traderId);       

        IEnumerable<ProductCategoryServiceModel> AllCategories();

        IEnumerable<ProductSubCategoryServiceModel> AllSubCategories();

        bool IsProductPublic(string id);

        bool ProductExists(string Id);

        bool CategoryExists(int categoryId);

        bool SubCategoryExists(int subCategoryId, int categoryId);

        bool SubCategoryParticipateInCategory(string subCategory,string category);

        bool SubCategoryIsValid(string subCategory,string category);

        bool DeleteProduct(
            string id,
            bool IsAdmin=false);

        bool ReviveProduct(string id);

        void ChangeVisibility(string id);





    }
}
