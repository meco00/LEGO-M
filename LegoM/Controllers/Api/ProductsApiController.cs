namespace LegoM.Controllers.Api
{
    using LegoM.Models.Api.Products;
    using LegoM.Services.Products;
    using LegoM.Services.Products.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/products")]
    public class ProductsApiController:ControllerBase
    {
        private readonly IProductsService products;

        public ProductsApiController(IProductsService products)
        => this.products = products;

        [HttpGet]
        public ActionResult<ProductQueryModel> All([FromQuery] AllProductsApiRequestModel query)
        => this.products.All(
             query.Category,
             query.SubCategory,
             query.SearchTerm,
             query.CurrentPage,
             query.ProductsPerPage,
             query.ProductSorting);                       
    }
}
