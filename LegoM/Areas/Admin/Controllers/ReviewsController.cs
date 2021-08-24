namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Areas.Admin.Models.Reviews;
    using LegoM.Services.Reviews;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController:AdminController
    {
        private readonly IReviewService reviews;

        public ReviewsController(IReviewService reviews)
        => this.reviews = reviews;

        public IActionResult All([FromQuery] ReviewsQueryModel query)
        {
              var queryResult = this.reviews.All(
           query.SearchTerm,
           query.CurrentPage,
           ReviewsQueryModel.ReviewsPerPage,
           IsPublicOnly: false);

            query.Reviews = queryResult.Reviews;
            query.TotalReviews = queryResult.TotalReviews;

            return this.View(query);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.reviews.ChangeVisibility(id);

            return RedirectToAction(nameof(All));
        }
    }
}
