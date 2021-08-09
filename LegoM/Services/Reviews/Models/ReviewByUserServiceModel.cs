namespace LegoM.Services.Reviews.Models
{
    public class ReviewByUserServiceModel:IReviewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Rating { get; set; }

        public string PublishedOn { get; set; }
    }
}
