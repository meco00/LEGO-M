namespace LegoM.Areas.Admin.Models.Comments
{
    using LegoM.Services.Comments.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CommentsQueryModel
    {
        public const int CommentsPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalComments { get; set; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }
    }
}
