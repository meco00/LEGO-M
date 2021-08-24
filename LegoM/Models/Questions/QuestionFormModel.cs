namespace LegoM.Models.Questions
{
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.Question;

    public class QuestionFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name = "Question:")]
        public string Content { get; set; }
    }
}
