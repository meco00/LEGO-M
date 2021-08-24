namespace LegoM.Services.Products
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Products.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductsService : IProductsService
    {
        private readonly LegoMDbContext data;
        private readonly IConfigurationProvider mapper;



        public ProductsService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public ProductQueryModel All(
            string category = null,
            string subCategory = null,
            string searchTerm = null,
            int currentPage = 1,
            int productsPerPage = int.MaxValue,
            ProductSorting productSorting = ProductSorting.Newest,
            bool IsPublicOnly = true,
            bool IsDeleted=false
            )
        {
           

             var  productsQuery = this.data.Products
                .Where(x=> (!IsPublicOnly || x.IsPublic) && x.IsDeleted==IsDeleted)
                .AsQueryable();

            ;

            if (!string.IsNullOrEmpty(category))
            {
                if (!string.IsNullOrEmpty(subCategory) &&
                    this.SubCategoryParticipateInCategory(subCategory, category))
                {

                    productsQuery = productsQuery
                   .Where(x => x.Category.Name.Contains(category) && x.SubCategory.Name.Contains(subCategory));

                }
                else
                {

                    productsQuery = productsQuery
                    .Where(x => x.Category.Name.Contains(category));

                }
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

            };

            var totalProducts = productsQuery.Count();

            var products = GetProducts(
                 productsQuery
                  .Skip((currentPage - 1) * productsPerPage)
                    .Take(productsPerPage)
                );


         


            return new ProductQueryModel
            {
                Products = products,
                CurrentPage = currentPage,
                ProductsPerPage = productsPerPage,
                TotalProducts = totalProducts,

            };
        }

        public IEnumerable<ProductServiceModel> Latest()
       => this.data.Products
              .Where(x=>x.IsPublic)
              .OrderByDescending(x => x.PublishedOn)
              .ProjectTo<ProductServiceModel>(this.mapper)
              .Take(5)
              .ToList();

        public string Create(string title,
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
                string traderId,
                bool IsPublic=false
            )
        {
            var productData = new Product()
            {
                Title = title,
                Description = description,
                Price = price,
                Quantity = quantity,
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                ProductCondition = productCondition,
                DeliveryTake = productDelivery,
                PublishedOn = DateTime.UtcNow,
                TraderId = traderId,
                IsPublic=IsPublic,
                IsDeleted=false

            };

            foreach (var imageUrl in new List<string>() { firstImageUrl, secondImageUrl, thirdImageUrl })
            {
                if (imageUrl != null)
                {
                    productData.Images.Add(new ProductImage()
                    {
                        ProductId = productData.Id,
                        ImageUrl = imageUrl
                    });
                }

            }

            data.Products.Add(productData);

            data.SaveChanges();

            return productData.Id;

        }

        public bool Edit(
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
          )
        {
            var productData = this.data.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == Id);

            if (productData == null)
            {
                return false;
            }

            if (productData.IsDeleted)
            {
                IsPublic = false;
            }

            productData.Title = title;
            productData.Description = description;
            productData.Price = price;
            productData.Quantity = quantity;
            productData.ProductCondition = productCondition;
            productData.DeliveryTake = productDelivery;
            productData.CategoryId = categoryId;
            productData.SubCategoryId = subCategoryId;
            productData.IsPublic = IsPublic ;

            var mainImage = productData.Images.FirstOrDefault();

            if (mainImage.ImageUrl != firstImageUrl)
            {
                mainImage.ImageUrl = firstImageUrl;
            }

            if (secondImageUrl != null)
            {
                var secondImage = productData.Images.Skip(1).Take(1).FirstOrDefault();

                if (secondImage == null)
                {
                    productData.Images.Add(new ProductImage() { ProductId = productData.Id, ImageUrl = secondImageUrl });
                }
                else if (secondImage.ImageUrl != secondImageUrl)
                {
                    secondImage.ImageUrl = secondImageUrl;
                }
            }
            if (thirdImageUrl != null)
            {
                var thirdImage = productData.Images.Skip(2).Take(1).FirstOrDefault();

                if (thirdImage == null)
                {
                    productData.Images.Add(new ProductImage() { ProductId = productData.Id, ImageUrl = thirdImageUrl });
                }
                else if (thirdImage.ImageUrl != thirdImageUrl)
                {
                    thirdImage.ImageUrl = thirdImageUrl;
                }
            }

            data.SaveChanges();

            return true;
        }
       
        public IEnumerable<ProductServiceModel> ByUser(string userId)
        => GetProducts(
            this.data
                    .Products
                      .Where(x => x.Trader.UserId == userId&& !x.IsDeleted)
            );

        public IEnumerable<ProductServiceModel> GetSimilarProducts(string Id)
        {
            var product = Details(Id);

            if (product==null)
            {
                return null;
            }

            var productFirstPartOfTitle = product.Title.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];

            return GetProducts(
                 this.data
                 .Products
                 .Where(x => x.Category.Name == product.CategoryName &&
                 x.SubCategory.Name == product.SubCategoryName &&
                 x.Title.Contains(productFirstPartOfTitle) && x.Title != product.Title)
                 .Take(5));
        }



        private  IEnumerable<ProductServiceModel> GetProducts(IQueryable<Product> productsQuery)
         => productsQuery
            .ProjectTo<ProductServiceModel>(this.mapper)
              .ToList();

        public IEnumerable<ProductCategoryServiceModel> AllCategories()
        => this.data
            .Categories
              .ProjectTo<ProductCategoryServiceModel>(this.mapper)
                .ToList();

        public bool SubCategoryExists(int subCategoryId, int categoryId)
        => this.data.SubCategories.Any(x => x.Id == subCategoryId && x.CategoryId == categoryId);

        public bool SubCategoryParticipateInCategory(string subCategory, string category)
         => this.data.SubCategories.Any(x => x.Name == subCategory && x.Category.Name == category);

        public bool SubCategoryIsValid(string subCategory, string category)
        {
            if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(subCategory))
            {
                if (!this.SubCategoryParticipateInCategory(subCategory, category))
                {
                    return false;
                }

            }
            else if (!string.IsNullOrEmpty(subCategory))
            {
                return false;
            }

            return true;

        }


        public bool CategoryExists(int categoryId)
        => this.data.Categories.Any(x => x.Id == categoryId);

        public ProductDetailsServiceModel Details(string Id)
        => this.data.Products
            .Where(x => x.Id == Id)
            .ProjectTo<ProductDetailsServiceModel>(this.mapper)
            .FirstOrDefault();

        public bool ProductIsByTrader(string id, string traderId)
        => this.data.Products
            .Any(x => x.Id == id && x.TraderId == traderId);

        public IEnumerable<ProductSubCategoryServiceModel> AllSubCategories()
        => this.data
            .SubCategories
             .ProjectTo<ProductSubCategoryServiceModel>(this.mapper)
                .ToList();

       public void ChangeVisibility(string id)
        {
            var product = this.data.Products.Find(id);

            product.IsPublic = !product.IsPublic;

            this.data.SaveChanges();
        }

        public bool ProductExists(string Id)
        => this.data.Products.Any(x => x.Id == Id);

        public bool DeleteProduct(
            string id,
            bool IsAdmin=false)
        {
           var product = this.data.Products.Find(id);

            if (product==null)
            {
                return false;
            }

            if (IsAdmin)
            {
                this.data.Products.Remove(product);
            }
            else
            {
                product.IsDeleted = true;
                product.IsPublic = false;
                product.DeletedOn = DateTime.UtcNow;

            }

            this.data.SaveChanges();

            return true;
        }


        public bool ReviveProduct(string id)
        {
            var product = this.data.Products.Find(id);

            if (product == null)
            {
                return false;
            }

            product.IsDeleted = false;
            product.DeletedOn = null;
            product.PublishedOn = DateTime.UtcNow;

            this.data.SaveChanges();

            return true;

        }

        public bool IsProductPublic(string id)
        => this.data.Products.Any(x => x.Id == id && x.IsPublic);
    }
}
