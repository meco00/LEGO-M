namespace LegoM.Controllers
{
    using LegoM.Areas.Admin;
    using LegoM.Infrastructure;
    using LegoM.Services.Products;
    using LegoM.Services.ShoppingCarts;
    using LegoM.Services.ShoppingCarts.Models;
    using LegoM.Services.Traders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;



    public class ShoppingCartController:Controller
    {
        private readonly IProductsService products;
        private readonly ITraderService traders;
        private readonly IShoppingCartService shoppingCarts;

        public ShoppingCartController(IProductsService products, IShoppingCartService shoppingCarts, ITraderService traders)
        {
            this.products = products;
            this.shoppingCarts = shoppingCarts;
            this.traders = traders;
        }

        [Authorize]
        public IActionResult Add(string id)
        {        
            var product = this.products.Details(id);

            if (product==null)
            {
                return NotFound();
            }

            string traderId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (traderId != null &&
                !isUserAdmin &&
                this.products.ProductIsByTrader(id, traderId))
            {
                return BadRequest();
            }


            if (product.Quantity == 0 || this.shoppingCarts.ItemExists(id,this.User.Id()))
            {
                return BadRequest();
            }


            this.shoppingCarts.Add(id, this.User.Id());


            this.TempData[WebConstants.GlobalMessageKey] = "Product was added succesfully to Shopping Cart!";

            return RedirectToAction(nameof(ProductsController.Details), "Products", new { id });


        }

        [Authorize]
        public IActionResult Mine()
        {         

            var cartItems = this.shoppingCarts.Mine(this.User.Id());

            return View(cartItems);

        }

        [Authorize]    
        public IActionResult Edit(int id)
        {
            if (!this.shoppingCarts.ItemIsByUser(id, this.User.Id()) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

            var cartItemQuantityProductQuantityModel = this.shoppingCarts.GetQuantityAndProductQuantity(id);

            if (cartItemQuantityProductQuantityModel == null)
            {
                return NotFound();
            }


            return View(cartItemQuantityProductQuantityModel);

        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id,CartItemServiceModel cartItemEdit )
        {
            var isUserAdmin = this.User.IsAdmin();

            if (!this.shoppingCarts.ItemIsByUser(id, this.User.Id()) && !isUserAdmin)
            {
                return BadRequest();
            }

           var IsEdited =  this.shoppingCarts.Edit(id, cartItemEdit.Quantity);

            if (!IsEdited)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Cart item quantity was edited succesfully!";

            if (isUserAdmin)
            {
                return RedirectToAction(nameof(Areas.Admin.Controllers.OrdersController.UnAccomplished), "Orders",new { area=AdminConstants.AreaName});
            }


            return RedirectToAction(nameof(Mine));
        }



        [Authorize]
        public IActionResult Delete(int id)
        {
            var isUserAdmin = this.User.IsAdmin();


            if (!this.shoppingCarts.ItemIsByUser(id,this.User.Id()) && !isUserAdmin)
            {
                return BadRequest();
            }

            var IsDeleted = this.shoppingCarts.Delete(id);

            if (!IsDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Product was deleted succesfully from Shopping Cart!";

            if (isUserAdmin)
            {
                return RedirectToAction(nameof(Areas.Admin.Controllers.OrdersController.UnAccomplished), "Orders", new { area = AdminConstants.AreaName });
            }

            return RedirectToAction(nameof(Mine));

        }


    }
}
