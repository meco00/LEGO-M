namespace LegoM.Controllers
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Infrastructure;
    using LegoM.Models.Traders;
    using LegoM.Services.Traders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    using static WebConstants;

    public class TradersController : Controller
    {

        private readonly ITraderService traders;

        public TradersController(ITraderService traders)
        => this.traders = traders;
        

        [Authorize]
        public IActionResult Become()
        {
            if (this.traders.IsUserTrader(this.User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeTraderFormModel merchant)
        {
            var userId = this.User.Id();

            if (this.traders.IsUserTrader(userId))
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(merchant);
            }

            this.traders.Create(
                 userId, 
                 merchant.Name, 
                 merchant.TelephoneNumber);


            TempData[GlobalMessageKey] = "Thank you for becoming Trader.";

            return RedirectToAction(nameof(ProductsController.All), "Products");
        }

     

    }
}
