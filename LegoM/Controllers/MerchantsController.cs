namespace LegoM.Controllers
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Infrastructure;
    using LegoM.Models.Merchants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    public class MerchantsController:Controller
    {
        private readonly LegoMDbContext data;

        public MerchantsController(LegoMDbContext data)
           => this.data = data;

        [Authorize]
        public IActionResult Become()
        {
            if (IsUserMerchant())
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeMerchantFormModel merchant)
        {
            if (IsUserMerchant())
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(merchant);
            }

            var userId = this.User.GetId();

            var merchantData = new Merchant
            {
                Name = merchant.Name,
                TelephoneNumber = merchant.TelephoneNumber,
                UserId = userId
            };

            data.Merchants.Add(merchantData);

            data.SaveChanges();

            return RedirectToAction("All", "Products");
        }

        private bool IsUserMerchant()
       => this.data.Merchants.Any(x => x.UserId == this.User.GetId());


    }
}
