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
            var review = this.reviews.ReviewById(id);

            if (review == null || review.GetInformation() != information || !(review.IsPublic))
            {
                return NotFound();
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(int id,string information, CommentFormModel comment)
        {
            var review = this.reviews.ReviewById(id);

            if (review == null || review.GetInformation() != information || !(review.IsPublic))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return this.View(comment);
            }

            var IsUserAdmin = this.User.IsAdmin();

            this.comments.Create(
                id,
                this.User.Id(),
                comment.Content,
                IsUserAdmin);

            this.TempData[WebConstants.GlobalMessageKey] = $"Your comment was added  { (IsUserAdmin ? string.Empty : "and is awaiting for approval!") }";

            

            return RedirectToAction("Details", "Reviews", new { id = review.Id, information = review.GetInformation() });
        }

        




    }
}
