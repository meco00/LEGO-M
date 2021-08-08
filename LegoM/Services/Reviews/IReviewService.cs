namespace LegoM.Services.Reviews
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Reviews.Models;
    using System.Collections.Generic;

    public interface IReviewService
    {
        void Create(
            string productId,
            string userId,
            ReviewType rating,
            string content,
            string title
            );

        ReviewDetailsServiceModel Details(int id);

        IEnumerable<ReviewServiceModel> All(string productId);

        ProductReviewsStatisticsServiceModel GetStatisticsForProduct(string productId);

        bool ReviewAlreadyExistsForUser(string productId, string userId);

    }
}
