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
            if (!this.merchants.IsMerchant(this.User.Id()))
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
            string merchantId = this.merchants.IdByUser(this.User.Id());

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

            this.products.Create(product.Title,
                product.Description,
                product.Price,
                product.Quantity,
                product.Condition.Value,
                product.Delivery.Value,
                merchantId,
                product.SubCategoriesIds);

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
            var myProducts=this.products.ByUser(this.User.Id());

            return this.View(myProducts);

        }

        [Authorize]
        public IActionResult Edit(string Id)
        {
            var userId = this.User.Id();


            if (!this.merchants.IsMerchant(this.User.Id()))
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }

            var product = this.products.Details(Id);

            if (product.UserId!=userId)
            {
                return Unauthorized();
            }



            return View(new ProductFormModel
            {
                Title=product.Title,
                Description=product.Description,
                Quantity=product.Quantity,
                Price=product.Price,
                Condition=Enum.Parse<ProductCondition>(product.Condition),
                Delivery=Enum.Parse<DeliveryTake>(product.Delivery),
                SubCategoriesIds=product.SubCategoriesIds,
                SubCategories = this.products.AllSubCategories(),
            });
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(string Id,ProductFormModel product)
        {
            string merchantId = this.merchants.IdByUser(this.User.Id());

            if (merchantId == null)
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
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

            if (!this.products.ProductIsByMerchant(Id,merchantId))
            {
                return BadRequest();
            }

          this.products.Edit(
                Id,
                product.Title,
                product.Description,
                product.Price,
                product.Quantity,
                product.Condition.Value,
                product.Delivery.Value,
                merchantId,
                product.SubCategoriesIds);

           

            return RedirectToAction(nameof(Mine));
        }

       

     


    }
}
