namespace LegoM.Controllers
{
    using AutoMapper;
    using LegoM.Infrastructure;
    using LegoM.Models.Reviews;
    using LegoM.Services.Products;
    using LegoM.Services.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController:Controller
    {
        private readonly IProductsService products;
        private readonly IReviewService reviews;

        private readonly IMapper mapper;

        public ReviewsController(IProductsService products, IReviewService reviews, IMapper mapper)
        {
            this.products = products;
            this.reviews = reviews;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Add(string productId)
        {
            if (!this.products.ProductExists(productId))
            {
                return BadRequest();
            }

            var review = this.reviews.ReviewByUser(productId, this.User.Id());

            if (review !=null)
            {
                return RedirectToAction("Details",new { id=review.Id, information=review.Information});

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

            ;


            var reviewModel = this.reviews.ReviewByUser(productId, this.User.Id());

            if (reviewModel != null)
            {
                return RedirectToAction("Details", new { id = reviewModel.Id, information = reviewModel.Information });

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

            var reviewFormModel = this.mapper.Map<ReviewFormModel>(review);

            return this.View(reviewFormModel);

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

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.reviews.ReviewIsByUser(id, this.User.Id()))
            {
                return BadRequest();
            }



           return this.View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id,ReviewDeleteFormModel deleteModel)
        {
            ;
            if (!this.reviews.ReviewIsByUser(id, this.User.Id()))
            {
                return BadRequest();
            }

            if (!deleteModel.SureToDelete)
            {
                return RedirectToAction(nameof(Mine));
            }

          var isDeleted = this.reviews.Delete(id);

            if (!isDeleted)
            {
                return BadRequest();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Succesfully deleted review.";

            return RedirectToAction(nameof(Mine));
        }

        




    }
}
