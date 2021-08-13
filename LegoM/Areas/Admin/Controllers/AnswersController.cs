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
            ;
            var answers = this.answers.All();

            return View(answers);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.answers.ChangeVisibility(id);


            return RedirectToAction(nameof(All));

        }

        
        public IActionResult Delete(int id)
        {
            var deleted= this.answers.Delete(id); 

            if (!deleted)
            {
                return NotFound();
            }
            
          
            return RedirectToAction(nameof(All));

        }


    }
}
