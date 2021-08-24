namespace LegoM.Services.Questions.Models
{
    public class QuestionDetailsServiceModel:IQuestionModel
    {
        public int Id { get; set; }

        public string Content { get; init; }

        public string UserName { get; init; }

        public string PublishedOn { get; init; }

        public string ProductId { get; init; }

        public string ProductTitle { get; init; }

        public string ProductImage { get; init; }

        public string ProductPrice { get; init; }

        public int ProductCondition { get; init; }

        public int AnswersCount { get; init; }

        public bool IsPublic { get; init; }
    }
}
