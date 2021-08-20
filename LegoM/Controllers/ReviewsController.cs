namespace LegoM.Controllers
{
    using AutoMapper;
    using LegoM.Infrastructure;
    using LegoM.Models.Reviews;
    using LegoM.Services.Comments;
    using LegoM.Services.Products;
    using LegoM.Services.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController:Controller
    {
        private readonly IProductsService products;
        private readonly IReviewService reviews;
        private readonly ICommentService comments;

        private readonly IMapper mapper;

        public ReviewsController(IProductsService products, IReviewService reviews,ICommentService comments,IMapper mapper)
        {
            this.products = products;
            this.reviews = reviews;
            this.mapper = mapper;
            this.comments = comments;
        }

        [Authorize]
        public IActionResult Add(string Id)
        {
            var IsUserAdmin = this.User.IsAdmin();

            if (!this.products.IsProductPublic(Id))
            {
                return BadRequest();
            }

            var reviewModel = this.reviews.ReviewByProductAndUser(Id, this.User.Id());

            if (reviewModel != null && !IsUserAdmin)
            {

                return RedirectToAction("Details", new { id = reviewModel.Id, information = reviewModel.GetInformation() });

            }       
         

            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(string Id,ReviewFormModel review)
        {
            ;
            var IsUserAdmin = this.User.IsAdmin();

            if (!this.products.IsProductPublic(Id))
            {
                return BadRequest();
            }
           
            var reviewModel = this.reviews.ReviewByProductAndUser(Id, this.User.Id());

            if (reviewModel != null && !IsUserAdmin)
            {
               
                return RedirectToAction("Details", new { id = reviewModel.Id, information = reviewModel.GetInformation() });

            }
            

            if (!(ModelState.IsValid))
            {
                return this.View(review);

            }


             

            this.reviews.Create(
                Id,
                this.User.Id(),
                review.Rating.Value,
                review.Content,
                review.Title,
                IsUserAdmin
                );

            this.TempData[WebConstants.GlobalMessageKey] = $"Review was added { (this.User.IsAdmin() ? string.Empty : "and is awaiting for approval!") } ";


            return RedirectToAction(nameof(ProductsController.Details), "Products" , new {id=Id });
        }


        public IActionResult Details(int id,string information)
        {
            ;

            var review = this.reviews.Details(id);

            if (review==null || review.GetInformation() != information)
            {
                return NotFound();
            }

            var reviewComments = this.comments.CommentsOfReview(id);
            

            return this.View(new ReviewDetailsWithCommentsModel 
            {
                Review=review,
                Comments=reviewComments
            });

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
            ;

            var review = this.reviews.Details(id);

            if (review == null)
            {
                return NotFound();
            }

            if (!this.reviews.ReviewIsByUser(id, this.User.Id()) && !this.User.IsAdmin())
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
            var IsUserAdmin = this.User.IsAdmin();

            if (!this.reviews.ReviewIsByUser(id,this.User.Id())&& !IsUserAdmin)
            {
                return BadRequest();
            }
          
          

            if (!(ModelState.IsValid))
            {
                return this.View(review);

            }

           

            var isReviewEdited = this.reviews.Edit
                (id, 
                review.Rating.Value, 
                review.Content, 
                review.Title,
                IsUserAdmin
                );

            if (!isReviewEdited)
            {
                return BadRequest();
            }

            this.TempData[WebConstants.GlobalMessageKey] = $"Review was edited { (IsUserAdmin ? string.Empty : "and is awaiting for approval!") } ";


            return RedirectToAction(nameof(ReviewsController.Mine), "Reviews");

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            ;
            if (!this.reviews.ReviewIsByUser(id, this.User.Id())&& !this.User.IsAdmin())
            {
                return BadRequest();
            }

            if (!this.reviews.ReviewExists(id))
            {
                return NotFound();
            }



           return this.View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id,ReviewDeleteFormModel deleteModel)
        {
            ;
            if (!this.reviews.ReviewIsByUser(id, this.User.Id()) && !this.User.IsAdmin())
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

            this.TempData[WebConstants.GlobalMessageKey] = $"Your review was deleted { (this.User.IsAdmin() ? string.Empty : "and is awaiting for approval!") } ";

            return RedirectToAction(nameof(Mine));
        }

        




    }
}
