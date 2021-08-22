namespace LegoM.Areas.Admin.Models.Answers
{
    using LegoM.Services.Answers.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AnswersQueryModel
    {
        public const int AnswersPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalAnswers { get; set; }

        public IEnumerable<AnswerServiceModel> Answers { get; set; }
    }
}
