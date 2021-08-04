namespace LegoM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Infrastructure;
    using LegoM.Models.Products;
    using LegoM.Services.Merchants;
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static WebConstants;

    public class ProductsController:Controller
    {
        private readonly IProductsService products;
        private readonly IMerchantService merchants;
        private readonly IMapper mapper;

        private readonly LegoMDbContext data;


        public ProductsController(LegoMDbContext data, IProductsService products, IMerchantService merchants, IMapper mapper)
        {
            this.data = data;
            this.products = products;
            this.merchants = merchants;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.merchants.IsMerchant(this.User.Id()) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }


            return View(new ProductFormModel
            {
                Categories = this.products.AllCategories(),
                SubCategories = this.products.AllSubCategories()
            });
        }

      

        [HttpPost]
        [Authorize]
        public IActionResult Add(ProductFormModel product)
        {
            string merchantId = this.merchants.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null && !isUserAdmin)
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }

            if (!product.AgreeOnTermsOfPolitics && !isUserAdmin)
            {
                this.ModelState.AddModelError(nameof(product.AgreeOnTermsOfPolitics), "You must agree before submiting.");
            }
            if (!this.products.CategoryExists(product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exists.");
            }
            else if (!this.products.SubCategoryExists(product.SubCategoryId,product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.SubCategoryId), "SubCategory is not valid.");
            }

            if (!ModelState.IsValid)
            {

                product.Categories = this.products.AllCategories();
                product.SubCategories = this.products.AllSubCategories();

                return View(product);
            }

            this.products.Create(product.Title,
                product.Description,
                product.FirstImageUrl,
                product.SecondImageUrl,
                product.ThirdImageUr,
                product.Price,
                product.Quantity,
                product.CategoryId,
                product.SubCategoryId,
                product.Condition.Value,
                product.Delivery.Value,
                merchantId
                );

            TempData[GlobalMessageKey] = "Sucessfully added product!";

            return RedirectToAction(nameof(All));

        }

        public IActionResult All([FromQuery]ProductsQueryModel query)
        {
            var queryResult = this.products.All(
            query.Category,
            query.SearchTerm,
            query.CurrentPage,
            ProductsQueryModel.ProductsPerPage,
            query.ProductSorting);

            var productCategories = this.products.Categories();

            query.Products = queryResult.Products;
            query.Categories = productCategories;
            query.TotalProducts = queryResult.TotalProducts;


            return this.View(query);
        }


        [Authorize]
        public IActionResult Mine()
        {
            var myProducts=this.products.ByUser(this.User.Id());

            if (User.IsAdmin())
            {
                return RedirectToAction(nameof(All)); 
            }

            return this.View(myProducts);

        }

        [Authorize]
        public IActionResult Edit(string Id)
        {
            var userId = this.User.Id();

            var isUserAdmin = this.User.IsAdmin();


            if (!this.merchants.IsMerchant(userId) && !isUserAdmin)
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }

            var product = this.products.Details(Id);

            if (product.UserId!=userId && !isUserAdmin)
            {
                return Unauthorized();
            }

            var productForm = this.mapper.Map<ProductFormModel>(product);

            productForm.Categories = this.products.AllCategories();
            productForm.SubCategories = this.products.AllSubCategories();


            ;


            return View(productForm);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(string Id,ProductFormModel product)
        {
            string merchantId = this.merchants.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (merchantId == null&& !isUserAdmin)
            {
                return RedirectToAction(nameof(MerchantsController.Become), "Merchants");
            }

            if (!this.products.ProductIsByMerchant(Id, merchantId) && !isUserAdmin)
            {
                return BadRequest();
            }


            if (!this.products.CategoryExists(product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.CategoryId), "Category does not exists.");
            }
            else if (!this.products.SubCategoryExists(product.SubCategoryId, product.CategoryId))
            {
                this.ModelState.AddModelError(nameof(product.SubCategoryId), "SubCategory is not valid.");
            }

            if (!ModelState.IsValid)
            {

                product.Categories = this.products.AllCategories();
                product.SubCategories = this.products.AllSubCategories();

                return View(product);
            }

           

          this.products.Edit(
                Id,
                product.Title,
                product.Description,
                product.FirstImageUrl,
                product.SecondImageUrl,
                product.ThirdImageUr,
                product.Price,
                product.Quantity,
                product.CategoryId,
                product.SubCategoryId,
                product.Condition.Value,
                product.Delivery.Value,
                merchantId);


            TempData[GlobalMessageKey] = "Sucessfully edited product!";

            return RedirectToAction(nameof(All));
        }


        public IActionResult Details(string id)
        {
            var product = this.products.Details(id);




            return this.View(product);

        }





    }
}
