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
                    .Where(x => x.Category.Name.Contains(category));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {

                productsQuery = productsQuery
                   
                    

                    .Where(x => x.SubCategory.Name.ToLower().Contains(searchTerm.ToLower())
                    || x.Category.Name.ToLower().Contains(searchTerm.ToLower())
                    || x.Title.ToLower().Contains(searchTerm.ToLower())
                    || (x.Category.Name + " " + x.SubCategory.Name).ToLower().Contains(searchTerm.ToLower())
                    || x.Description.ToLower().Contains(searchTerm.ToLower()));

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
                string categoryId,
                string subCategoryId,
                ProductCondition productCondition,
                DeliveryTake productDelivery,
                string merchantId)
        {
            var productData = new Product()
            {
                Title = title,
                Description = description,
                Price = price,
                Quantity = quantity,
                CategoryId=categoryId,
                SubCategoryId=subCategoryId,
                ProductCondition = productCondition,
                DeliveryTake = productDelivery,
                PublishedOn = DateTime.UtcNow,
                MerchantId = merchantId
            };



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
          string categoryId,
          string subCategoryId,
          ProductCondition productCondition,
          DeliveryTake productDelivery,
          string merchantId
          )
        {
            var productData = this.data.Products.FirstOrDefault(x => x.Id == Id);

            if (productData == null)
            {
                return false;
            }

            productData.Title = title;
            productData.Description = description;
            productData.Price = price;
            productData.Quantity = quantity;
            productData.ProductCondition = productCondition;
            productData.DeliveryTake = productDelivery;
            productData.CategoryId = categoryId;
            productData.SubCategoryId = subCategoryId;

            data.SaveChanges();

            return true;
        }

        public IEnumerable<string> Categories()
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

        public IEnumerable<ProductCategoryServiceModel> AllCategories()
        => this.data
            .Categories
              .Select(x => new ProductCategoryServiceModel
              {
                  Id = x.Id,
                  Name = x.Name
              })
                .ToList();

        public bool SubCategoryExists(string subCategoryId, string categoryId)
        => this.data.SubCategories.Any(x => x.Id==subCategoryId && x.CategoryId==categoryId);

        public bool CategoryExists(string categoryId)
        => this.data.Categories.Any(x => x.Id == categoryId);

        public ProductDetailsServiceModel Details(string Id)
        => this.data.Products
            .Where(x => x.Id == Id)
            .Select(x => new ProductDetailsServiceModel
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Quantity=x.Quantity,
                CategoryId=x.CategoryId,
                SubCategoryId=x.SubCategoryId,
                Delivery=x.DeliveryTake.ToString(),
                Condition = x.ProductCondition.ToString(),
                Description=x.Description,
                MerchantId=x.MerchantId,
                MerchantName=x.Merchant.Name,
                UserId=x.Merchant.UserId,
                                                          
            })
            .FirstOrDefault();

        public bool ProductIsByMerchant(string id, string merchantId)
        => this.data.Products
            .Any(x => x.Id == id && x.MerchantId == merchantId);

        public IEnumerable<ProductSubCategoryServiceModel> AllSubCategories()
        => this.data
            .SubCategories
              .Select(x => new ProductSubCategoryServiceModel
              {
                  Id = x.Id,
                  CategoryId=x.CategoryId,
                  Name = x.Name
    })
                .ToList();
}
}
