namespace LegoM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductsController:Controller
    {
        private readonly LegoMDbContext data;

        public ProductsController(LegoMDbContext data)
           => this.data = data;


        public IActionResult Add() => View(new AddProductFormModel
        {
           
            SubCategories = this.GetSubCategoriesOfCategory(),
          
            
        });

        [HttpPost]
        public IActionResult Add(AddProductFormModel product)
        {
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
                PublishedOn=DateTime.UtcNow
            };

            foreach (var subCategoryId in product.SubCategoriesIds.Distinct())
            {
                var subCategory = this.data.SubCategories.FirstOrDefault(x => x.Id == subCategoryId);

                if (subCategory== null)
                {
                    continue;
                }

                ;

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
            var productsQuery = this.data.Products.AsQueryable();

            ;

            if (!string.IsNullOrEmpty(query.Category))
            {
                productsQuery = productsQuery
                    .Where(x => x.ProductsSubCategories.Any(x => x.SubCategory.Category.Name == query.Category));
            }

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {

                productsQuery = productsQuery
                    .Include(x=>x.ProductsSubCategories)
                    .ThenInclude(x=>x.SubCategory)
                    .ThenInclude(x=>x.Category)
                    .ToList()
                    
                    .Where(x => x.ProductsSubCategories.Any(sb => sb.SubCategory.Name.ToLower().Contains(query.SearchTerm.ToLower()))
                    || x.ProductsSubCategories.Any(sb => sb.SubCategory.Category.Name.ToLower().Contains(query.SearchTerm.ToLower()))
                    || x.Title.ToLower().Contains(query.SearchTerm.ToLower())
                    || (x.Title +" "+x.ProductCondition.ToString()).ToLower().Contains(query.SearchTerm.ToLower())
                    || x.Description.ToLower().Contains(query.SearchTerm.ToLower())).AsQueryable();

            }

            productsQuery = query.ProductSorting switch
            {

              
                ProductSorting.NameAlphabetically => productsQuery=productsQuery.OrderBy(x=>x.Title),
                ProductSorting.NameDescending => productsQuery=productsQuery.OrderByDescending(x=>x.Title),
                ProductSorting.PriceAscending => productsQuery=productsQuery.OrderBy(x=>x.Price),
                ProductSorting.PriceDescending => productsQuery=productsQuery.OrderByDescending(x=>x.Price),
                ProductSorting.Newest or _=> productsQuery.OrderByDescending(x=>x.PublishedOn)

                //TODO : After implementation of Rating implement sorting by rating criteria too

            };

            var totalProducts = productsQuery.Count();

            var products = productsQuery
                .Skip((query.CurrentPage -1)*ProductsQueryModel.ProductsPerPage)
                .Take(ProductsQueryModel.ProductsPerPage)
                .Select(x => new ProductListingViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Condition = x.ProductCondition.ToString()
            })
                .ToList();

            var productCategories = this.data.Categories.Select(x => x.Name).Distinct().ToList();

            query.Products = products;
            query.Categories = productCategories;
            query.TotalProducts = totalProducts;
                

            return this.View(query);
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
