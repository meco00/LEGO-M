namespace LegoM.Areas.Admin.Controllers
{
    using LegoM.Services.Answers;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AnswersController:AdminController
    {
        private readonly IAnswerService answers;

        public AnswersController(IAnswerService answers)
        {
            this.answers = answers;
        }


        public IActionResult All()
        {
            var questions = this.answers.All();

            return View(questions);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.answers.ChangeVisibility(id);


            return RedirectToAction(nameof(All));

        }


    }
}
