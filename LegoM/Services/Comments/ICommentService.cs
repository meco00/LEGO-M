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
            string content,
            bool IsPublic = false);

        IEnumerable<CommentServiceModel> CommentsOfReview(int reviewId);

        CommentQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int commentsPerPage = int.MaxValue,
            bool IsPublicOnly = true);

        void ChangeVisibility(int id);

        bool Delete(int id);

    }
}
