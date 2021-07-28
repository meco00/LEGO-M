namespace LegoM.Services.Products
{
  using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductsService : IProductsService
    {
        private readonly LegoMDbContext data;


        public ProductsService(LegoMDbContext data)
        => this.data = data;

       


        public ProductQueryServiceModel All(string category,
            string searchTerm,
            int currentPage,
            int productsPerPage,
            ProductSorting productSorting
            )
        {
            var productsQuery = this.data.Products.AsQueryable();

            ;

            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery
                    .Where(x => x.ProductsSubCategories.Any(x => x.SubCategory.Category.Name == category));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {

                productsQuery = productsQuery
                    .Include(x => x.ProductsSubCategories)
                    .ThenInclude(x => x.SubCategory)
                    .ThenInclude(x => x.Category)
                    .ToList()

                    .Where(x => x.ProductsSubCategories.Any(sb => sb.SubCategory.Name.ToLower().Contains(searchTerm.ToLower()))
                    || x.ProductsSubCategories.Any(sb => sb.SubCategory.Category.Name.ToLower().Contains(searchTerm.ToLower()))
                    || x.Title.ToLower().Contains(searchTerm.ToLower())
                    || (x.Title + " " + x.ProductCondition.ToString()).ToLower().Contains(searchTerm.ToLower())
                    || x.Description.ToLower().Contains(searchTerm.ToLower())).AsQueryable();

            }

            productsQuery = productSorting switch
            {


                ProductSorting.NameAlphabetically => productsQuery = productsQuery.OrderBy(x => x.Title),
                ProductSorting.NameDescending => productsQuery = productsQuery.OrderByDescending(x => x.Title),
                ProductSorting.PriceAscending => productsQuery = productsQuery.OrderBy(x => x.Price),
                ProductSorting.PriceDescending => productsQuery = productsQuery.OrderByDescending(x => x.Price),
                ProductSorting.Newest or _ => productsQuery.OrderByDescending(x => x.PublishedOn)

                //TODO : After implementation of Rating implement sorting by rating criteria too

            };

            var totalProducts = productsQuery.Count();

            var products = GetProducts(
                 productsQuery
                  .Skip((currentPage - 1) * productsPerPage)
                    .Take(productsPerPage)
                );
                

            var productCategories = this.data.Categories.Select(x => x.Name).Distinct().ToList();

            return new ProductQueryServiceModel
            {
                Products = products,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                TotalProducts=totalProducts
            };
        }

        public string Create(string title,
            string description,
            decimal price,
            byte quantity, 
            ProductCondition productCondition,
            DeliveryTake productDelivery,
            string merchantId, 
            IEnumerable<string> subCategoriesIds)
        {
            var productData = new Product()
            {
                Title = title,
                Description = description,
                Price = price,
                Quantity = quantity,
                ProductCondition = productCondition,
                DeliveryTake = productDelivery,
                PublishedOn = DateTime.UtcNow,
                MerchantId = merchantId
            };

            foreach (var subCategoryId in subCategoriesIds.Distinct())
            {

                var isSubCategoryExists = this.SubCategoriesExists(new string[] { subCategoryId });

                if (!isSubCategoryExists)
                {
                    continue;
                }

                productData.ProductsSubCategories.Add(new ProductSubCategory
                {
                    Product = productData,
                    SubCategoryId = subCategoryId
                });
            }

            data.Products.Add(productData);

            data.SaveChanges();

            return productData.Id;
        }

        public bool Edit(
          string Id,
          string title,
          string description,
          decimal price,
          byte quantity,
          ProductCondition productCondition,
          DeliveryTake productDelivery,
          string merchantId,
          IEnumerable<string> subCategoriesIds)
        {
            var productData = this.data.Products
                .Include(x=>x.ProductsSubCategories)
                .FirstOrDefault(x=>x.Id==Id);

            if (productData==null)
            {
                return false;
            }

            productData.Title = title;
            productData.Description = description;
            productData.Price = price;
            productData.Quantity = quantity;
            productData.ProductCondition = productCondition;
            productData.DeliveryTake = productDelivery;
            productData.PublishedOn = DateTime.UtcNow;

            var productSubCategories = productData.ProductsSubCategories.Select(x => x.SubCategoryId).ToList();

            bool isInputSubCategoriesEqual = Enumerable.SequenceEqual(productSubCategories, subCategoriesIds);

            ;

            if (isInputSubCategoriesEqual)
            {
                data.SaveChanges();

                return true;
            }

            data.ProductsSubCategories.RemoveRange(productData.ProductsSubCategories);

            foreach (var subCategoryId in subCategoriesIds.Distinct())
            {

                var isSubCategoryExists = this.SubCategoriesExists(new string[] { subCategoryId });

                if (!isSubCategoryExists)
                {
                    continue;
                }

                productData.ProductsSubCategories.Add(new ProductSubCategory
                {
                    Product = productData,
                    SubCategoryId = subCategoryId
                });
            }

          

            data.SaveChanges();

            return true;
        }

        public IEnumerable<string> AllCategories()
        => this.data
            .Categories
            .Select(x => x.Name)
            .Distinct()
            .OrderBy(ca=>ca)
            .ToList();

        public IEnumerable<ProductServiceModel> ByUser(string userId)
        => GetProducts(
            this.data
                    .Products
                      .Where(x => x.Merchant.UserId == userId)
            );
            


        private static IEnumerable<ProductServiceModel> GetProducts(IQueryable<Product> productsQuery)
         => productsQuery
            .Select(x => new ProductServiceModel()
             {
                 Id = x.Id,
                 Title = x.Title,
                 Price = x.Price,
                 Condition = x.ProductCondition.ToString()
             })
              .ToList();

        public IEnumerable<ProductSubCategoryServiceModel> AllSubCategories()
        => this.data
            .SubCategories
              .Select(x => new ProductSubCategoryServiceModel
              {
                  Id = x.Id,
                  Name = x.Name
              })
                .ToList();

        public bool SubCategoriesExists(IEnumerable<string> subCategoriesIds)
        => this.data.SubCategories.Any(x => subCategoriesIds.Contains(x.Id));

        public ProductDetailsServiceModel Details(string Id)
        => this.data.Products
            .Where(x => x.Id == Id)
            .Select(x => new ProductDetailsServiceModel
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Quantity=x.Quantity,
                Delivery=x.DeliveryTake.ToString(),
                Condition = x.ProductCondition.ToString(),
                Description=x.Description,
                MerchantId=x.MerchantId,
                MerchantName=x.Merchant.Name,
                UserId=x.Merchant.UserId,
                SubCategoriesIds=x.ProductsSubCategories.Select(x => x.SubCategoryId).ToList()

                                                          
            })
            .FirstOrDefault();

        public bool ProductIsByMerchant(string id, string merchantId)
        => this.data.Products
            .Any(x => x.Id == id && x.MerchantId == merchantId);
    }
}
