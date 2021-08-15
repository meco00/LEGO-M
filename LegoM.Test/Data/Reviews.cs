namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using static Comments;

    public static class Reviews
    {
        public const string TestContent = nameof(TestContent);

        public const string DEFAULT_TITLE = "TestReview";
        public const int DEFAULT_RATING = 5;
        public static DateTime DEFAULT_DATE = new DateTime(1, 1, 1);

        public static string GetInformation(
           
            string title=DEFAULT_TITLE,        
            int rating=DEFAULT_RATING

            )
         => String.Concat(title + "-" + rating + "-" + DEFAULT_DATE.ToString("dd MMM yyy"));

       public static List<Review> GetReviews(int count = 5,
            bool isPublic = true,
            bool sameUser = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };


            var reviews = Enumerable
               .Range(1, count)
               .Select(i => new Review
               {
                   Id = i,
                   Title = DEFAULT_TITLE,
                   Rating = LegoM.Data.Models.Enums.ReviewType.Excellent,
                   Content = TestContent,
                   IsPublic = isPublic,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Product = new Product
                   {
                       Title = "Test",
                       ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New
                   },
               })
               .ToList();


            return reviews;
        }

    }
}
