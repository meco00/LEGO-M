namespace LegoM.Services.Reviews
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class ReviewService : IReviewService
    {
        private readonly LegoMDbContext data;
        private readonly IConfigurationProvider mapper;

        public ReviewService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }



        public void Create(string productId, string userId, ReviewType rating, string content, string title)
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

        public IEnumerable<ReviewServiceModel> All(string productId)
        => this.data.Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.PublishedOn)
            .ProjectTo<ReviewServiceModel>(mapper)
            .ToList();

        public ReviewDetailsServiceModel Details(int id)
        => this.data.Reviews.Where(x => x.Id == id)
           .ProjectTo<ReviewDetailsServiceModel>(mapper)
            .FirstOrDefault();

        public IEnumerable<ReviewListingServiceModel> ByUser(string userId)
        => this.data.Reviews.Where(x => x.UserId == userId)
        .ProjectTo<ReviewListingServiceModel>(mapper)
        .ToList();



        public bool Edit(int id, ReviewType rating, string content, string title)
        {

            var review = this.data.Reviews.FirstOrDefault(x => x.Id == id);

            if (review == null)
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

        public bool Delete(int id)
        {
            var review = this.data.Reviews.FirstOrDefault(x => x.Id == id);

            if (review == null)
            {
                return false;
            }


            this.data.Reviews.Remove(review);

            this.data.SaveChanges();

            return true;
        }

        public ReviewsProductStatisticsServiceModel GetStatisticsForProduct(string productId)
        {

            var reviews = this.All(productId);

            if (reviews.Count() == 0)
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

            return new ReviewsProductStatisticsServiceModel
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

        public ReviewByUserServiceModel ReviewByUser(string productId, string userId)
        => this.data.Reviews
            .Where(x => x.ProductId == productId && x.UserId == userId)
            .ProjectTo<ReviewByUserServiceModel>(mapper)
            .FirstOrDefault();

        public ReviewByUserServiceModel ReviewById(int id)
         => this.data.Reviews
            .Where(x => x.Id == id)
            .ProjectTo<ReviewByUserServiceModel>(mapper)
            .FirstOrDefault();

        private static string ValidateTitle(ReviewType rating, string title)
        {
            if (title == null)
            {
                title = rating.ToString();
            }

            return title;
        }

        public bool ReviewIsByUser(int id, string userId)
        => this.data.Reviews.Any(x => x.Id == id && x.UserId == userId);

        public bool ReviewExists(int id)
        => this.data.Reviews.Any(x => x.Id == id);



    }


}
