namespace LegoM.Services.Questions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionDetailsServiceModel:IQuestionModel
    {
        public string Content { get; init; }

        public string UserName { get; init; }

        public string PublishedOn { get; init; }

        public string ProductId { get; init; }

        public string ProductTitle { get; init; }

        public string ProductImage { get; init; }

        public string ProductPrice { get; init; }

        public int ProductCondition { get; init; }

        public string ProductPublishedOn { get; init; }
    }
}
