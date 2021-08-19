namespace LegoM.Services.Reviews.Models
{
    using System.Collections.Generic;

    public class ReviewQueryModel
    {
        public int CurrentPage { get; init; }

        public int ReviewsPerPage { get; init; }

        public int TotalReviews { get; init; }

        public IEnumerable<ReviewServiceModel> Reviews { get; init; }
    }
}
