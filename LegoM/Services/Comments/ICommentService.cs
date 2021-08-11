namespace LegoM.Services.Comments
{
    using LegoM.Services.Answers.Models;
    using LegoM.Services.Comments.Models;
    using System.Collections.Generic;

    public interface ICommentService
    {
        void Create(
            int reviewId,
            string userId,
            string content);

        IEnumerable<CommentServiceModel> CommentsOfReview(int reviewId);

        void ChangeVisibility(int id);

    }
}
