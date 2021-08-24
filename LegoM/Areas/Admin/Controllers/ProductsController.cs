namespace LegoM.Areas.Admin.Controllers
{
    using AutoMapper;
    using LegoM.Areas.Admin.Models.Products;
    using LegoM.Models.Products;
    using LegoM.Services.Products;
    using LegoM.Services.Reports;
    using Microsoft.AspNetCore.Mvc;


    public class ProductsController:AdminController
    {
        private readonly IProductsService products;
        private readonly IReportsService reports;

        private readonly IMapper mapper;

        public ProductsController(IProductsService products, IMapper mapper, IReportsService reports)
        {
            this.products = products;
            this.mapper = mapper;
            this.reports = reports;
        }



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
            this.products.ChangeVisibility(id);

            return RedirectToAction(nameof(this.Existing));
        }

        public IActionResult Revive(string id)
        {
            this.products.ReviveProduct(id);

            return RedirectToAction(nameof(this.Existing));
        }

        public IActionResult Reports(string id)
        {
            var product = this.products.Details(id);

            if (product==null)
            {
                return NotFound();
            }

            var productModel = this.mapper.Map<ProductModel>(product);

            var reports = this.reports.All(productId: id).Reports;

            return View(new ProductReportsDetailsModel
            {
                Product = productModel,
                Reports = reports
            });
        }
    }
}
