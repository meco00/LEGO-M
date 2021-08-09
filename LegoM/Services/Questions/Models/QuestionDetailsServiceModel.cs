namespace LegoM.Services.Questions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionDetailsServiceModel:IQuestionModel
    {
        public string Title { get;}

        public string UserName { get; }

        public string PublishedOn { get; }

        public string ProductId { get;}

        public string ProductTitle { get; }

        public string ProductImage { get; }

        public string ProductPrice { get;}

        public int ProductCondition { get; }

        public string ProductPublishedOn { get; }
    }
}
