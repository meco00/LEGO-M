namespace LegoM.Services.Reviews.Models
{
  

    public class ProductReviewsStatisticsServiceModel
    {
        public string Rating { get; set; }

        public int TotalReviews { get; set; }

        public int FiveStarRatings { get; set; }
        public int FourStarRatings { get; set; }
        public int ThreeStarRatings { get; set; }
        public int TwoStarRatings { get; set; }
        public int OneStarRatings { get; set; }

        public string ProductTitle { get; init; }

        
    }
}
