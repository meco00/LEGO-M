namespace LegoM.Services.Comments.Models
{
    using System.Collections.Generic;

    public class CommentQueryModel
    {
        public int CurrentPage { get; init; }

        public int CommentsPerPage { get; init; }

        public int TotalComments { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; init; }
    }
}
