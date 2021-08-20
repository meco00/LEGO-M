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
            

            if (!(ModelState.IsValid))
            {
                return this.View(question);

            }

            var IsUserAdmin = this.User.IsAdmin();

            this.questions.Create(
                Id,
                this.User.Id(),
                question.Content,
                IsUserAdmin);

            this.TempData[WebConstants.GlobalMessageKey] = $"Your question was added  { (IsUserAdmin ? string.Empty : "and is awaiting for approval!") }";

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

            if (!this.questions.QuestionIsByUser(id,this.User.Id()) && !this.User.IsAdmin())
            {
                return BadRequest();
            }

          var isSuccesfullyDeleted = this.questions.Delete(id);

            if (!isSuccesfullyDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = $"Your question was deleted { (this.User.IsAdmin() ? string.Empty : "and is awaiting for approval!") } ";

            return RedirectToAction(nameof(Mine));

        }







    }
}
