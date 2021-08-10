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

        bool Edit(
            int id,
            ReviewType rating,
            string content,
            string title
            );

        ReviewDetailsServiceModel Details(int id);

        IEnumerable<ReviewListingServiceModel> ByUser(string userId);

        IEnumerable<ReviewServiceModel> All(string productId);

        ReviewsProductStatisticsServiceModel GetStatisticsForProduct(string productId);

        ReviewByUserServiceModel ReviewByUser(string productId, string userId);

        ReviewByUserServiceModel ReviewById(int id );

        bool ReviewExists(int id);

        bool ReviewIsByUser(int id, string userId);     

        bool Delete(int id);

    }
}
