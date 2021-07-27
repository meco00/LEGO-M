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
    using LegoM.Services.Merchants;
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductsController:Controller
    {
        private readonly IProductsService products;
        private readonly IMerchantService merchants;
        private readonly LegoMDbContext data;


        public ProductsController(LegoMDbContext data, IProductsService products, IMerchantService merchants)
        {
            this.data = data;
            this.products = products;
            this.merchants = merchants;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.merchants.IsMerchant(this.User.GetId()))
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }


            return View(new ProductFormModel
            {
                SubCategories = this.products.AllSubCategories(),
            });
        }

      

        [HttpPost]
        [Authorize]
        public IActionResult Add(ProductFormModel product)
        {
            string merchantId = this.merchants.GetMerchantId(this.User.GetId());

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
            else if (!this.products.SubCategoriesExists(product.SubCategoriesIds))
            {
                this.ModelState.AddModelError(nameof(product.SubCategoriesIds), "SubCategories does not exists.");
            }
            else if (product.SubCategoriesIds.Count() > 10)
            {
                this.ModelState.AddModelError(nameof(product.SubCategoriesIds), "Product can participate only in 10 SubCategories");
            }

            if (!ModelState.IsValid)
            {

                product.SubCategories = this.products.AllSubCategories();

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

                var isSubCategoryExists = this.products.SubCategoriesExists(new string[] { subCategoryId});

                if (!isSubCategoryExists)
                {
                    continue;
                }

                productToImport.ProductsSubCategories.Add(new ProductSubCategory
                {
                    Product = productToImport,
                    SubCategoryId = subCategoryId
                });
            }

            data.Products.Add(productToImport);

            data.SaveChanges();

            return RedirectToAction(nameof(Mine));

        }

        public IActionResult All([FromQuery]ProductsQueryModel query)
        {
           var queryResult= this.products.All(
           query.Category,
           query.SearchTerm,
           query.CurrentPage,
           ProductsQueryModel.ProductsPerPage,
           query.ProductSorting);

            var productCategories = this.products.AllCategories();

            query.Products = queryResult.Products;
            query.Categories = productCategories;
            query.TotalProducts = queryResult.TotalProducts;
                

            return this.View(query);
        }


        [Authorize]
        public IActionResult Mine()
        {
            var myProducts=this.products.ByUser(this.User.GetId());

            return this.View(myProducts);

        }

        [Authorize]
        public IActionResult Edit(string Id)
        {

        }

       

     


    }
}
