namespace LegoM.Services.Questions.Models
{
    public class QuestionListingServiceModel:IQuestionModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImage { get; set; }

        public int ProductCondition { get; set; }

        public int AnswersCount { get; init; }
    }
}
