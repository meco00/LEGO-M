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

        public IEnumerable<ProductServiceModel> Latest()
       => this.data.Products
              .OrderByDescending(x => x.PublishedOn)
              .ProjectTo<ProductServiceModel>(this.mapper)
              .Take(3)
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
                MerchantId = merchantId,
               
            };          

            foreach (var imageUrl in new List<string>() { firstImageUrl,secondImageUrl,thirdImageUrl})
            {
                if (imageUrl !=null)
                {
                    productData.Images.Add(new ProductImage()
                    {
                        ProductId = productData.Id,
                        ImageUrl = imageUrl
                    });
                }

            }


            ;




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
          string merchantId
          )
        {
            var productData = this.data.Products.Include(x=>x.Images).FirstOrDefault(x => x.Id == Id);

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

          var mainImage=productData.Images.FirstOrDefault();

            if (mainImage.ImageUrl != firstImageUrl)
            {
                mainImage.ImageUrl = firstImageUrl;
            }

            if (secondImageUrl != null)
            {
                var secondImage = productData.Images.Where(x=>x.isDeleted==false).Skip(1).Take(1).FirstOrDefault();

                if (secondImage==null)
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
                var thirdImage = productData.Images.Where(x => x.isDeleted == false).Skip(2).Take(1).FirstOrDefault();

                if (thirdImage == null)
                {
                    productData.Images.Add(new ProductImage() { ProductId = productData.Id, ImageUrl = thirdImageUrl });
                }
                else if (thirdImage.ImageUrl != thirdImageUrl)
                {
                    thirdImage.ImageUrl = thirdImageUrl;
                }
            }

            ;


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
                 Condition = x.ProductCondition.ToString(),
                 MainImageUrl=x.Images.Select(x=>x.ImageUrl).FirstOrDefault()
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

        public bool SubCategoryExists(int subCategoryId, int categoryId)
        => this.data.SubCategories.Any(x => x.Id==subCategoryId && x.CategoryId==categoryId);

        public bool CategoryExists(int categoryId)
        => this.data.Categories.Any(x => x.Id == categoryId);

        public ProductDetailsServiceModel Details(string Id)
        => this.data.Products
            .Where(x => x.Id == Id)
            .ProjectTo<ProductDetailsServiceModel>(this.mapper)          
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
