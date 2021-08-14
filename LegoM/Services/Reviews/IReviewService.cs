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
            string title,
            bool IsPublic = false
            );

        ReviewDetailsServiceModel Details(int id);     

        IEnumerable<ReviewListingServiceModel> ByUser(string userId);

        IEnumerable<ReviewServiceModel> All(string productId);

        IEnumerable<ReviewServiceModel> All();

        ReviewsProductStatisticsServiceModel GetStatisticsForProduct(string productId);

        ReviewServiceModel ReviewByProductAndUser(string productId, string userId);

        ReviewServiceModel ReviewById(int id);

        void ChangeVisibility(int id);

        bool ReviewExists(int id);

        bool ReviewIsByUser(int id, string userId);     

        bool Delete(int id);


    }
}
