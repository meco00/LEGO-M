namespace LegoM.Models.Questions
{
    using LegoM.Services.Answers.Models;
    using LegoM.Services.Questions.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionDetailsAndAnswersModel
    {
      public  QuestionDetailsServiceModel Question { get; init; }

       public IEnumerable<AnswerServiceModel> Answers { get; init; }
    }
}
