namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Mvc;


    public class ProductsController:AdminController
    {
        private readonly IProductsService products;

        public ProductsController(IProductsService products)
        => this.products = products;
        


        public IActionResult All() => View(this.products.All(isPublicOnly: false).Products);


        public IActionResult ChangeVisibility(string id)
        {
            this.products.ChangeVisibility(id);


           return RedirectToAction(nameof(All));

        }
    }

}
