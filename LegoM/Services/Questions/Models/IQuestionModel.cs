namespace LegoM.Services.Questions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IQuestionModel
    {
         int ProductCondition { get; }

         int AnswersCount { get; }

         string PublishedOn { get; }
    }
}
