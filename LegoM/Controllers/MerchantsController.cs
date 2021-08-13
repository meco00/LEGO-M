namespace LegoM.Controllers
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Infrastructure;
    using LegoM.Models.Merchants;
    using LegoM.Services.Merchants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    using static WebConstants;

    public class MerchantsController : Controller
    {

        private readonly IMerchantService merchants;

        public MerchantsController(IMerchantService merchants)
        => this.merchants = merchants;
        

        [Authorize]
        public IActionResult Become()
        {
            if (this.merchants.IsUserMerchant(this.User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeMerchantFormModel merchant)
        {
            var userId = this.User.Id();

            if (this.merchants.IsUserMerchant(userId))
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(merchant);
            }

            this.merchants.Create(
                 userId, 
                 merchant.Name, 
                 merchant.TelephoneNumber);


            TempData[GlobalMessageKey] = "Thank you for becoming Merchant.";

            return RedirectToAction(nameof(ProductsController.All), "Products");
        }

     

    }
}
