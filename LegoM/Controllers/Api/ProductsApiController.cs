﻿namespace LegoM.Controllers.Api
{
    using LegoM.Data;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Api.Products;
    using LegoM.Services.Products;
    using LegoM.Services.Products.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    [ApiController]
    [Route("api/products")]
    public class ProductsApiController:ControllerBase
    {
        private readonly IProductsService products;

        public ProductsApiController(IProductsService products)
        => this.products = products;


        [HttpGet]
        public ActionResult<ProductQueryServiceModel> All([FromQuery] AllProductsApiRequestModel query)
        => this.products.All(
             query.Category,
             query.SubCategory,
             query.SearchTerm,
             query.CurrentPage,
             query.ProductsPerPage,
             query.ProductSorting);


           
        


         
        
    }
}
