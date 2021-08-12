namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    public class ProductsController:AdminController
    {
        private readonly IProductsService products;

        public ProductsController(IProductsService products)
        => this.products = products;


        
        public IActionResult All()
        {
           var products = this.products.All(isPublicOnly: false).Products;

           return  View(products);
        }

        public IActionResult Deleted()
        {
          var products =  this.products.DeletedProducts();


            return View(products);

        }

        public IActionResult ChangeVisibility(string id)
        {
            ;
            this.products.ChangeVisibility(id);


           return RedirectToAction(nameof(All));

        }


        public IActionResult Revive(string id)
        {
            this.products.ReviveProduct(id);


            return RedirectToAction(nameof(All));

        }


    }

}
