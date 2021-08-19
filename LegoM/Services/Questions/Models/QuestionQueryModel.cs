namespace LegoM.Services.Questions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionQueryModel
    {
        public int CurrentPage { get; init; }

        public int QuestionsPerPage { get; init; }

        public int TotalQuestions { get; init; }

        public IEnumerable<QuestionServiceModel> Questions { get; init; }
    }
}
