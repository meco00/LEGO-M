namespace LegoM.Services.Answers.Models
{
    using System.Collections.Generic;

    public class AnswerQueryModel
    {
        public int CurrentPage { get; init; }

        public int AnswersPerPage { get; init; }

        public int TotalAnswers { get; init; }

        public IEnumerable<AnswerServiceModel> Answers { get; init; }
    }
}
