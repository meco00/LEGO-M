namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Models.Answers;
    using LegoM.Services.Answers;
    using LegoM.Services.Questions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AnswersController:Controller
    {
        private readonly IAnswerService answers;
        private readonly IQuestionsService questions;

        public AnswersController(IQuestionsService questions, IAnswerService answers)
        {
            this.questions = questions;
            this.answers = answers;
        }


        [Authorize]
        public IActionResult Add(int id)
        {
            ;
            if (!this.questions.QuestionExists(id))
            {
                return BadRequest();
            }

            var questionModel = this.questions.QuestionById(id);

            if (!questionModel.IsPublic)
            {
                return BadRequest();
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(int id,AnswerFormModel answer)
        {
            ;
            if (!this.questions.QuestionExists(id))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.View(answer);
            }

               ;
            var questionModel = this.questions.QuestionById(id);

            if (!questionModel.IsPublic)
            {
                return BadRequest();
            }



            this.answers.Create(
                id,
                this.User.Id(),
                answer.Content);

            this.TempData[WebConstants.GlobalMessageKey] = "Your answer was added and it is awaiting for approval!";

         



            return RedirectToAction("Details","Questions", new { id = questionModel.Id, information = questionModel.GetInformation() });

            
        }






    }
}
