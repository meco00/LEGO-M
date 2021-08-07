namespace LegoM.Models.Reviews
{
    using LegoM.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Review;

    public class ReviewFormModel
    {

        [Required(ErrorMessage = "Please select an rating from the list.")]
        [EnumDataType(typeof(ReviewType))]
        public ReviewType? Rating { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name = "Review:")]
        public string Content { get; set; }

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        public string Title { get; set; }


    }
}
