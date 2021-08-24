namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Services.Favourites;
    using LegoM.Services.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class FavouritesController:Controller
    {
        private readonly IFavouriteService favourites;
        private readonly IProductsService products;

        public FavouritesController(IFavouriteService favourites, IProductsService products)
        {
            this.favourites = favourites;
            this.products = products;
        }

        [Authorize]
        public IActionResult Add(string id)
        {
            if (!this.products.ProductExists(id))
            {
                return NotFound();
            }
            var userId = this.User.Id();

            if (this.User.IsAdmin() || this.favourites.IsFavouriteExists(id,userId))
            {
                return BadRequest();
            }

            this.favourites.Add(id, userId);

            this.TempData[WebConstants.GlobalMessageKey] = "Product was added succesfully to favourites!";

            return RedirectToAction(nameof(ProductsController.Details), "Products", new { id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();

            if (this.User.IsAdmin() || !this.favourites.IsFavouriteByUser(id, userId))
            {
                return BadRequest();
            }

            this.favourites.Delete(id);     

            this.TempData[WebConstants.GlobalMessageKey] = "Product was deleted succesfully from favourites!";


            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Mine()
        {
            var favourites = this.favourites.Mine(this.User.Id());

            return View(favourites);
        }
    }
}
