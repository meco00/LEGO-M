namespace LegoM.Services.Questions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionByUserServiceModel:IQuestionModel
    {
        public int Id { get; set; }

        public int ProductCondition { get; set; }

        public int AnswersCount { get; set; }

        public string PublishedOn { get; set; }
    }
}
