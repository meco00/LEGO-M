namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Models.Reviews;
    using LegoM.Services.Products;
    using LegoM.Services.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewsController:Controller
    {
        private readonly IProductsService products;
        private readonly IReviewService reviews;

        public ReviewsController(IProductsService products, IReviewService reviews)
        {
            this.products = products;
            this.reviews = reviews;
        }

        [Authorize]
        public IActionResult Add(string productId)
        {
            if (!this.products.ProductExists(productId))
            {
                return BadRequest();
            }

            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(string productId,ReviewFormModel review)
        {
            if (!this.products.ProductExists(productId))
            {
                return BadRequest();
            }

           
            ;

            if (!(ModelState.IsValid))
            {
                return this.View(review);

            }


            this.reviews.Create(
                productId,
                this.User.Id(),
                review.Rating.Value,
                review.Content,
                review.Title
                );

            this.TempData[WebConstants.GlobalMessageKey] = "Succesfully created review to product";

            return RedirectToAction("Details", "Products" , new {id=productId });
        }


    }
}
