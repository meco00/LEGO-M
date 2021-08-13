namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Models.Comments;
    using LegoM.Services.Comments;
    using LegoM.Services.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentsController:Controller
    {
        private readonly IReviewService reviews;
        private readonly ICommentService comments;

        public CommentsController(IReviewService reviews, ICommentService comments)
        {
            this.reviews = reviews;
            this.comments = comments;
        }

        [Authorize]
        public IActionResult Add(int id,string information)
        {
            var review = this.reviews.Details(id);

            if (review == null || review.GetInformation() != information)
            {
                return NotFound();
            }



            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(int id, CommentFormModel comment)
        {
            ;
            if (!this.reviews.ReviewExists(id))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.View(comment);
            }

            this.comments.Create(id, this.User.Id(), comment.Content);

            this.TempData[WebConstants.GlobalMessageKey] = "Your comment was added and it is awaiting for approval!";

            var review = this.reviews.ReviewById(id);

            return RedirectToAction("Details", "Reviews", new { id = review.Id, information = review.GetInformation() });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {

            var isUserAdmin = this.User.IsAdmin();

            if (isUserAdmin)
            {

                this.comments.Delete(id);

            }

            return RedirectToAction(nameof(Index), "Home");

        }




    }
}
