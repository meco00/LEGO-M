namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Areas.Admin.Models.Comments;
    using LegoM.Services.Comments;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentsController:AdminController
    {
        private readonly ICommentService comments;

        public CommentsController(ICommentService comments)
        {
            this.comments = comments;
        }


        public IActionResult All([FromQuery] CommentsQueryModel query)
        {
            var queryResult = this.comments.All(
            query.SearchTerm,
            query.CurrentPage,
            CommentsQueryModel.CommentsPerPage,
            IsPublicOnly: false);

            query.Comments = queryResult.Comments;
            query.TotalComments = queryResult.TotalComments;

            return this.View(query);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.comments.ChangeVisibility(id);


            return RedirectToAction(nameof(All));

        }

        
        public IActionResult Delete(int id)
        {
            var deleted = this.comments.Delete(id);

            if (!deleted)
            {

                return NotFound();

            }

            this.TempData[WebConstants.GlobalMessageKey] = "Comment was deleted successfully!";


            return RedirectToAction(nameof(All));

        }


    }
}
