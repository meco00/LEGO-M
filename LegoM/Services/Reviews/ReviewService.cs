namespace LegoM.Services.Reviews
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
   

    public class ReviewService : IReviewService
    {
        private readonly LegoMDbContext data;

        public ReviewService(LegoMDbContext data)
        {
            this.data = data;
        }

        public IEnumerable<ReviewServiceModel> All(string productId)
        => this.data.Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.PublishedOn)
            .Select(x => new ReviewServiceModel
            {
                Id=x.Id,
                Title = x.Title,
                Content = x.Content,
                UserName = x.User.FullName,
                PublishedOn = x.PublishedOn.ToString("MM MMM yyy"),
                Rating = (int)x.Rating
            })
            .ToList();

        public IEnumerable<ReviewListingServiceModel> ByUser(string userId)
        => this.data.Reviews.Where(x => x.UserId == userId).Select(x => new ReviewListingServiceModel
        {
            Id=x.Id,
            Title = x.Title,
            Content = x.Content,
            Rating = (int)x.Rating,
            PublishedOn = x.PublishedOn.ToString("MM MMM yyy"),
            ProductId=x.ProductId,
            ProductTitle=x.Product.Title,
            ProductImage= x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault(),

        }).ToList();

        public void Create(string productId,string userId, ReviewType rating, string content, string title)
        {
            title = ValidateTitle(rating, title);

            var review = new Review
            {
                Rating = rating,
                Content = content,
                Title = title,
                ProductId = productId,
                UserId = userId,
                PublishedOn = DateTime.UtcNow

            };


            this.data.Add(review);

            this.data.SaveChanges();


        }

        

        public ReviewDetailsServiceModel Details(int id)
        => this.data.Reviews.Where(x => x.Id == id)
            .Select(x => new ReviewDetailsServiceModel
            {
                Title=x.Title,
                Rating=(int)x.Rating,
                Content=x.Content,
                UserName=x.User.UserName,
                UserId=x.UserId,
                PublishedOn=x.PublishedOn.ToString("MM MMM yyy"),
                ProductImage=x.Product.Images.Select(x=>x.ImageUrl).FirstOrDefault(),
                ProductId=x.ProductId,
                ProductPrice=x.Product.Price.ToString("f2"),
                ProductTitle=x.Product.Title,
            })
            .FirstOrDefault();

        public bool Edit(int id, ReviewType rating, string content, string title)
        {

            var review = this.data.Reviews.FirstOrDefault(x => x.Id == id);

            if (review==null)
            {
                return false;
            }

            title = ValidateTitle(rating, title);


            review.Title = title;
            review.Rating = rating;
            review.Content = content;

            this.data.SaveChanges();

            return true;


        }

        public ProductReviewsStatisticsServiceModel GetStatisticsForProduct(string productId)
        {

            var reviews = this.All(productId);

            if (reviews.Count()==0)
            {
                return null;
            }

            var reviewsCount = reviews.Count();

            var rating = ((decimal)(reviews.Sum(x => x.Rating)) / (decimal)reviewsCount).ToString("F2");

            var fiveStarRatings = reviews.Where(x => x.Rating == 5).Count();
            var fourStarRatings = reviews.Where(x => x.Rating == 4).Count();
            var threeStarRatings = reviews.Where(x => x.Rating == 3).Count();
            var twoStarRatings = reviews.Where(x => x.Rating == 2).Count();
            var oneStarRatings = reviews.Where(x => x.Rating == 1).Count();

            return new ProductReviewsStatisticsServiceModel
            {
                Rating = rating,
                TotalReviews = reviewsCount,
                FiveStarRatings = fiveStarRatings,
                FourStarRatings = fourStarRatings,
                ThreeStarRatings = threeStarRatings,
                TwoStarRatings = twoStarRatings,
                OneStarRatings = oneStarRatings
            };
        }

        public bool ReviewAlreadyExistsForUser(string productId, string userId)
        => this.data.Reviews.Any(x => x.ProductId == productId && x.UserId == userId);

        private static string ValidateTitle(ReviewType rating, string title)
        {
            if (title == null)
            {
                title = rating.ToString();
            }

            return title;
        }

        public bool ReviewIsByUser(int id, string userId)
        =>this.data.Reviews.Any(x=>x.Id==id&&x.UserId==userId);
    }

   
}
