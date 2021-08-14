namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Models.Questions;
    using LegoM.Services.Answers;
    using LegoM.Services.Products;
    using LegoM.Services.Questions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController:Controller
    {
        private readonly IProductsService products;
        private readonly IQuestionsService questions;
        private readonly IAnswerService answers;

        public QuestionsController(IProductsService products, IQuestionsService questions, IAnswerService answers)
        {
            this.products = products;
            this.questions = questions;
            this.answers = answers;
        }

        [Authorize]
        public IActionResult Add(string Id)
        {
            if (!this.products.IsProductPublic(Id))
            {
                return BadRequest();
            }

            var question = this.questions.QuestionByProductAndUser(Id, this.User.Id());



            if (question != null)
            {
                return RedirectToAction("Details", new { id = question.Id, information = question.GetInformation() });

            }

            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(string Id,QuestionFormModel question)
        {
            ;
            if (!this.products.IsProductPublic(Id))
            {
                return BadRequest();
            }

            var questionModel = this.questions.QuestionByProductAndUser(Id, this.User.Id());

            if (questionModel != null)
            {
                return RedirectToAction("Details", new { id = questionModel.Id, information = questionModel.GetInformation() });

            }
            

            if (!(ModelState.IsValid))
            {
                return this.View(question);

            }


            this.questions.Create(
                Id,
                this.User.Id(),
                question.Content);

            this.TempData[WebConstants.GlobalMessageKey] = "Your question was added and it is awaiting for approval!";

            return RedirectToAction(nameof(ProductsController.Details), "Products", new { id = Id });

        }

        public IActionResult Details(int id, string information)
        {
            ;

            var question = this.questions.Details(id);

            if (question == null || question.GetInformation() != information)
            {
                return NotFound();
            }


          var questionAnswers = this.answers.AnswersOfQuestion(id);

           

            return this.View(new QuestionDetailsWithAnswersModel
            { 
               Question=question,
               Answers=questionAnswers               
              
            });

        }

        [Authorize]
        public IActionResult Mine()
        {
            ;
            
            var questions = this.questions.Mine(this.User.Id());


            return this.View(questions);

        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            ;

            if (!this.questions.QuestionIsByUser(id,this.User.Id()))
            {
                return NotFound();
            }

          var isSuccesfullyDeleted = this.questions.Delete(id);

            if (!isSuccesfullyDeleted)
            {
                return BadRequest();
            }

            this.TempData[WebConstants.GlobalMessageKey] = $"Your question was deleted { (this.User.IsAdmin() ? string.Empty : "and is awaiting for approval!") } ";

            return RedirectToAction(nameof(Mine));

        }







    }
}
