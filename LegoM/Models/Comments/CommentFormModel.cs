namespace LegoM.Models.Comments
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Comment;

    public class CommentFormModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name ="Comment :")]
        public string Content { get; set; }
    }
}
