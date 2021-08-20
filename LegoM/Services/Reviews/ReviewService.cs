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



        public void Create(
            string productId,
            string userId, 
            ReviewType rating,
            string content,
            string title,
            bool IsPublic = false)
        {
            title = ValidateTitle(rating, title);

            var review = new Review
            {
                Rating = rating,
                Content = content,
                Title = title,
                ProductId = productId,
                UserId = userId,
                PublishedOn = DateTime.UtcNow,
                IsPublic= IsPublic

            };


            this.data.Add(review);

            this.data.SaveChanges();


        }



        public ReviewDetailsServiceModel Details(int id)
        => this.data.Reviews.Where(x => x.Id == id)
           .ProjectTo<ReviewDetailsServiceModel>(mapper)
            .FirstOrDefault();

        public IEnumerable<ReviewListingServiceModel> ByUser(string userId)
        => this.data.Reviews.Where(x => x.UserId == userId)
        .ProjectTo<ReviewListingServiceModel>(mapper)
        .ToList();



        public bool Edit(int id, ReviewType rating, string content, string title,
            bool IsPublic=false)
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
            review.IsPublic = IsPublic;

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

            var reviews = this.All(productId : productId).Reviews;        

            var reviewsCount = reviews.Count();

            var rating = reviewsCount == 0 ? "0" : ((decimal)(reviews.Sum(x => x.Rating)) / (decimal)reviewsCount).ToString("F2");

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

        public ReviewServiceModel ReviewByProductAndUser(string productId, string userId)
        => this.data.Reviews
            .Where(x => x.ProductId == productId && x.UserId == userId)
            .ProjectTo<ReviewServiceModel>(mapper)
            .FirstOrDefault();

        public ReviewServiceModel ReviewById(int id)
         => this.data.Reviews
            .Where(x => x.Id == id)
            .ProjectTo<ReviewServiceModel>(mapper)
            .FirstOrDefault();


        public void ChangeVisibility(int id)
        {
           var review = this.data.Reviews.Find(id);

            if (review==null)
            {
                return;
            }

            review.IsPublic = !review.IsPublic;

            this.data.SaveChanges();
        }

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

        public ReviewQueryModel All(
            string searchTerm = null,
            int currentPage = 1, 
            int reviewsPerPage = int.MaxValue,
            ReviewType? reviewFiltering = null,
            bool IsPublicOnly = true,
            string productId = null)
        {
            var reviewsQuery = this.data.Reviews
                .Where(x => !IsPublicOnly || x.IsPublic)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productId))
            {

               reviewsQuery = reviewsQuery.Where(x => x.ProductId == productId);

            }


            if (!string.IsNullOrEmpty(searchTerm))
            {

                reviewsQuery = reviewsQuery
                                         .Where(x =>
                                         x.Content.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Title.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Product.Title.ToLower().Contains(searchTerm.ToLower()));

            }

         

            if (reviewFiltering.HasValue)
            {
                reviewsQuery = reviewFiltering switch
                {
                    ReviewType.NotRecommended => reviewsQuery = reviewsQuery.Where(x => x.Rating==ReviewType.NotRecommended),
                    ReviewType.Weak => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewType.Weak),
                    ReviewType.Average => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewType.Average),
                    ReviewType.Good => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewType.Good),
                    ReviewType.Excellent or _=> reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewType.Excellent),
                   
                };

            }


            var totalReviews = reviewsQuery.Count();

            var reviews = reviewsQuery
                  .Skip((currentPage - 1) * reviewsPerPage)
                    .Take(reviewsPerPage)
                    .OrderByDescending(x => x.PublishedOn)
                    .ProjectTo<ReviewServiceModel>(mapper)
                    .ToList();

            return new ReviewQueryModel
            {
                Reviews = reviews,
                CurrentPage = currentPage,
                TotalReviews = totalReviews,
                ReviewsPerPage = reviewsPerPage,
            };
        }
    }


}
