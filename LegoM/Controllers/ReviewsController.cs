namespace LegoM.Controllers
{
    using LegoM.Data.Models.Enums;
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

            if (this.reviews.ReviewAlreadyExistsForUser(productId,this.User.Id()))
            {
                return RedirectToAction("Details");

            }




            this.reviews.Create(
                productId,
                this.User.Id(),
                review.Rating.Value,
                review.Content,
                review.Title
                );

            this.TempData[WebConstants.GlobalMessageKey] = "Succesfully created review to product";

            return RedirectToAction(nameof(ProductsController.Details), "Products" , new {id=productId });
        }


        public IActionResult Details(int id,string information)
        {
            ;

            var review = this.reviews.Details(id);

            if (review==null || review.GetInformation() != information)
            {
                return NotFound();
            }

            

            return this.View(review);

        }

        [Authorize]
        public IActionResult Mine()
        {
            ;
            var myReviews = this.reviews.ByUser(this.User.Id());


            return this.View(myReviews);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            var review = this.reviews.Details(id);

            if (review == null)
            {
                return NotFound();
            }

            if (review.UserId != this.User.Id())
            {
                return BadRequest();
            }

            return this.View(new ReviewFormModel
            {
                Title = review.Title,
                Rating = (ReviewType)review.Rating,
                Content = review.Content


            });

        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id,ReviewFormModel review)
        {

            ;


            if (!(ModelState.IsValid))
            {
                return this.View(review);

            }

            if (!this.reviews.ReviewIsByUser(id,this.User.Id()))
            {
                return BadRequest();
            }

            var isReviewEdited = this.reviews.Edit
                (id, 
                review.Rating.Value, 
                review.Content, 
                review.Title
                );

            if (!isReviewEdited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(ReviewsController.Mine), "Reviews");

        }


        




    }
}
