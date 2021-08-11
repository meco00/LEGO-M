using LegoM.Services.Questions;
using Microsoft.AspNetCore.Mvc;

namespace LegoM.Areas.Admin.Controllers
{
    public class QuestionsController:AdminController
    {
        private readonly IQuestionsService questions;

        public QuestionsController(IQuestionsService questions)
        {
            this.questions = questions;
        }


        public IActionResult All()
        {
            var questions = this.questions.All();

            return View(questions);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.questions.ChangeVisibility(id);


            return RedirectToAction(nameof(All));

        }



    }
}
