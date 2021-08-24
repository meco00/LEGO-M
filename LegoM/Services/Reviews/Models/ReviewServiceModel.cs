namespace LegoM.Services.Reviews.Models
{
    public class ReviewServiceModel:IReviewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        
        public string UserName { get; set; }

        public string ProductTitle { get; set; }

        public string PublishedOn { get; set; }

        public bool IsPublic { get; set; }

        public int Rating { get; set; }

        public int TotalComments { get; set; }  
    }
}
