namespace LegoM.Services.Reviews
{
    using LegoM.Data.Models.Enums;

    public interface IReviewService
    {
        void Create(
            string productId,
            string userId,
            ReviewType rating,
            string content,
            string title
            );

    }
}
