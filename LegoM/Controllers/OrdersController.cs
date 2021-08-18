namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Services.ShoppingCarts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    using static WebConstants;

    public class OrdersController:Controller
    {
        private readonly IShoppingCartService shoppingCarts;
       

        public OrdersController(IShoppingCartService shoppingCarts)
        {
            this.shoppingCarts = shoppingCarts;
        }

        [Authorize]
        public IActionResult Add()
        {
            ;
            var IsUserShoppingCartContainsAnyCartItem = this.shoppingCarts
                .UserHasAnyUnOrderedShoppingCartItem(this.User.Id());

            if (!IsUserShoppingCartContainsAnyCartItem)
            {
                this.TempData[GlobalErrorMessageKey] = new List<string>() { "Your shopping cart is empty" };

                return RedirectToAction(nameof(ShoppingCartController.Mine), "ShoppingCart");
            }

            var cartItemsErrorMessages = this.shoppingCarts.GetInformationAboutInvalidShoppingCartItemsOfUser(this.User.Id());

            if (cartItemsErrorMessages.Count > 0)
            {
                this.TempData[GlobalErrorMessageKey] = cartItemsErrorMessages;

                return RedirectToAction(nameof(ShoppingCartController.Mine), "ShoppingCart");

            }



            return View();

        }

    }
}
