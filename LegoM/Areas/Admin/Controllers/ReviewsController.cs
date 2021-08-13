namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Services.Reviews;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewsController:AdminController
    {
        private readonly IReviewService reviews;

        public ReviewsController(IReviewService reviews)
        => this.reviews = reviews;



        public IActionResult All() 
        {
            ;
            var reviews = this.reviews.All();

            return View(reviews);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.reviews.ChangeVisibility(id);


            return RedirectToAction(nameof(All));

        }


    }
}
