namespace LegoM.Services.Questions.Models
{
    using System.Collections.Generic;

    public class QuestionQueryModel
    {
        public int CurrentPage { get; init; }

        public int QuestionsPerPage { get; init; }

        public int TotalQuestions { get; init; }

        public IEnumerable<QuestionServiceModel> Questions { get; init; }
    }
}
