namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Models.Questions;
    using LegoM.Services.Products;
    using LegoM.Services.Questions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController:Controller
    {
        private readonly IProductsService products;
        private readonly IQuestionsService questions;

        public QuestionsController(IProductsService products, IQuestionsService questions)
        {
            this.products = products;
            this.questions = questions;
        }

        [Authorize]
        public IActionResult Add(string Id)
        {
            if (!this.products.ProductExists(Id))
            {
                return BadRequest();
            }

            var question = this.questions.QuestionByUser(Id, this.User.Id());

            if (question != null)
            {
                return RedirectToAction("Details", new { id = question.Id, information = question.Information });

            }

            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(string Id,QuestionFormModel question)
        {
            if (!this.products.ProductExists(Id))
            {
                return BadRequest();
            }

            ;

            if (!(ModelState.IsValid))
            {
                return this.View(question);

            }

            var questionModel = this.questions.QuestionByUser(Id, this.User.Id());

            if (questionModel != null)
            {
                return RedirectToAction("Details", new { id = questionModel.Id, information = questionModel.Information });

            }

            this.questions.Create(
                Id,
                this.User.Id(),
                question.Content);


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



            return this.View(question);

        }






    }
}
