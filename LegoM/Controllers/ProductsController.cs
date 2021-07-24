namespace LegoM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Infrastructure;
    using LegoM.Models.Products;
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductsController:Controller
    {
        private readonly IProductsService products;
        private readonly LegoMDbContext data;


        public ProductsController(LegoMDbContext data, IProductsService products)
        {
            this.data = data;
            this.products = products;
        }

        [Authorize]
        public IActionResult Add()
        {
            string userMerchantId = GetMerchantId();

            if (userMerchantId == null)
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }


            return View(new AddProductFormModel
            {

                SubCategories = this.GetSubCategoriesOfCategory(),


            });
        }

      

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddProductFormModel product)
        {
            string merchantId = GetMerchantId();

            if (merchantId == null)
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }

            if (!product.AgreeOnTermsOfPolitics)
            {
                this.ModelState.AddModelError(nameof(product.AgreeOnTermsOfPolitics), "You must agree before submiting.");
            }

            if (!product.SubCategoriesIds.Any())
            {
                
               this.ModelState.AddModelError(nameof(product.SubCategoriesIds), "You must select at least 1 SubCategory");
            }
            else if (!this.data.SubCategories.Any(x=>product.SubCategoriesIds.Contains(x.Id)))
            {
                this.ModelState.AddModelError(nameof(product.SubCategoriesIds), "SubCategories does not exists.");
            }
            else if (product.SubCategoriesIds.Count() > 10)
            {
                this.ModelState.AddModelError(nameof(product.SubCategoriesIds), "Product can participate only in 10 SubCategories");
            }

            if (!ModelState.IsValid)
            {

                product.SubCategories = this.GetSubCategoriesOfCategory();

                return View(product);
            }


            var productToImport = new Product()
            {
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ProductCondition = product.Condition.Value,
                DeliveryTake = product.Delivery.Value,
                PublishedOn=DateTime.UtcNow,
                MerchantId=merchantId
            };

            foreach (var subCategoryId in product.SubCategoriesIds.Distinct())
            {
                var subCategory = this.data.SubCategories.FirstOrDefault(x => x.Id == subCategoryId);

                if (subCategory== null)
                {
                    continue;
                }

                productToImport.ProductsSubCategories.Add(new ProductSubCategory
                {
                    Product = productToImport,
                    SubCategory = subCategory
                });
            }

            data.Products.Add(productToImport);

            data.SaveChanges();

            return RedirectToAction(nameof(All));

        }

        public IActionResult All([FromQuery]ProductsQueryModel query)
        {
           var queryResult= this.products.All(
           query.Category,
           query.SearchTerm,
           query.CurrentPage,
           ProductsQueryModel.ProductsPerPage,
           query.ProductSorting);

            var productCategories = this.products.AllProductCategories();

            query.Products = queryResult.Products;
            query.Categories = productCategories;
            query.TotalProducts = queryResult.TotalProducts;
                

            return this.View(query);
        }

        private string GetMerchantId()
        {
            return this.data.Merchants
                .Where(x => x.UserId == this.User.GetId())
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        private IEnumerable<ProductSubCategoryViewModel> GetSubCategoriesOfCategory()
       => this.data
            .SubCategories
              .Select(x => new ProductSubCategoryViewModel
              {
                  Id = x.Id,
                  Name = x.Name
              })
                .ToList();


    }
}
