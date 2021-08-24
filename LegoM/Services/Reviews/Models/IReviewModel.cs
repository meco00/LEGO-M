namespace LegoM.Services.Reviews.Models
{
    public interface IReviewModel
    {
        string Title { get; }
        int Rating { get; }
        string PublishedOn { get; }
    }
}
