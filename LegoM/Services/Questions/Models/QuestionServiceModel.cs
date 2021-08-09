namespace LegoM.Services.Questions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionServiceModel:IQuestionModel
    {
        public int  Id { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public int ProductCondition { get; set; }

        public string ProductPublishedOn { get; set; }

        public string PublishedOn { get; set; }
    }
}
