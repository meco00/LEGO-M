namespace LegoM.Areas.Admin.Models.Questions
{
    using LegoM.Services.Questions.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class QuestionsQueryModel
    {
        public const int QuestionsPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalQuestions { get; set; }

        public IEnumerable<QuestionServiceModel> Questions { get; set; }
    }
}
