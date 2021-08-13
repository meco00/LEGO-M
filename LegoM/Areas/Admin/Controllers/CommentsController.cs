namespace LegoM.Areas.Admin.Controllers
{
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


        public IActionResult All()
        {
            var comments = this.comments.All();

            return View(comments);
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

            return RedirectToAction(nameof(All));

        }


    }
}
