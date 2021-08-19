namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Models.Products;
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    public class ProductsController:AdminController
    {
        private readonly IProductsService products;

        public ProductsController(IProductsService products)
        => this.products = products;


        
        public IActionResult Existing([FromQuery]ProductsQueryModel query)
        {
            ;
            if (!this.products.SubCategoryIsValid(query.SubCategory, query.Category))
            {
                return BadRequest();
            }

            var queryResult = this.products.All(
            query.Category,
            query.SubCategory,
            query.SearchTerm,
            query.CurrentPage,
            ProductsQueryModel.ProductsPerPage,
            query.ProductSorting,
            IsPublicOnly: false);
         

            var categories = this.products.AllCategories();
            var subCategories = this.products.AllSubCategories();

            query.Products = queryResult.Products;
            query.Categories = categories;
            query.SubCategories = subCategories;
            query.TotalProducts = queryResult.TotalProducts;

            return this.View(query);
           
        }

        public IActionResult Deleted([FromQuery]ProductsQueryModel query)
        {
            ;
            if (!this.products.SubCategoryIsValid(query.SubCategory, query.Category))
            {
                return BadRequest();
            }

            var queryResult = this.products.All(
            query.Category,
            query.SubCategory,
            query.SearchTerm,
            query.CurrentPage,
            ProductsQueryModel.ProductsPerPage,
            query.ProductSorting,
            IsPublicOnly: false,
            IsDeleted: true);


            var categories = this.products.AllCategories();
            var subCategories = this.products.AllSubCategories();

            query.Products = queryResult.Products;
            query.Categories = categories;
            query.SubCategories = subCategories;
            query.TotalProducts = queryResult.TotalProducts;

            return this.View(query);

        }

        public IActionResult ChangeVisibility(string id)
        {
            ;
            this.products.ChangeVisibility(id);


           return RedirectToAction(nameof(this.Existing));

        }


        public IActionResult Revive(string id)
        {
            this.products.ReviveProduct(id);


            return RedirectToAction(nameof(this.Existing));

        }


    }

}
