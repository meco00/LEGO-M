namespace LegoM.Services.Reviews.Models
{
    public class ReviewListingServiceModel:IReviewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Rating { get; set; }

        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImage { get; set; }

        public int TotalComments { get; set; }
    }
}
