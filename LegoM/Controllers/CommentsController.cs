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
        public IActionResult Add(int id)
        {
            ;
            if (!this.reviews.ReviewExists(id))
            {
                return BadRequest();
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

            this.TempData[WebConstants.GlobalMessageKey] = "Succesfully created comment to question";

            var review = this.reviews.ReviewById(id);

            return RedirectToAction("Details", "Reviews", new { id = review.Id, information = review.GetInformation() });
        }


    }
}
