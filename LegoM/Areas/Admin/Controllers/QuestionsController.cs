using LegoM.Areas.Admin.Models.Questions;
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


        public IActionResult All([FromQuery]QuestionsQueryModel query)
        {
              var queryResult = this.questions.All(
           query.SearchTerm,
           query.CurrentPage,
           QuestionsQueryModel.QuestionsPerPage,
           IsPublicOnly: false);

            query.Questions = queryResult.Questions;
            query.TotalQuestions = queryResult.TotalQuestions;

            return this.View(query);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.questions.ChangeVisibility(id);


            return RedirectToAction(nameof(All));

        }



    }
}
