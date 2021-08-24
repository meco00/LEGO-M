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

        public const string Title = "TestReview";
        public const int Rating = 5;


        public static string GetInformation(
            string title=Title,        
            int rating=Rating)
         => String.Concat(title + "-" + rating + "-" + new DateTime(1, 1, 1).ToString("dd MMM yyy"));

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
                   Title = Title,
                   Rating = LegoM.Data.Models.Enums.ReviewType.Excellent,
                   Content = TestContent,
                   IsPublic = isPublic,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Review Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Product = new Product
                   {
                       Title = "Test",
                       ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New
                   },
                   ProductId = "TestId"
               })
               .ToList();


            return reviews;
        }


        public static List<Review> GetReviewsByProduct(string productId="TestId",int count=5)
          => Enumerable.Range(0, count).Select(i => new Review()
          {
              Title = Title,
              Rating = LegoM.Data.Models.Enums.ReviewType.Excellent,
              Content = TestContent,
              IsPublic = true,
              PublishedOn = new DateTime(1, 1, 1),
              ProductId =productId,
              User = new User
              {
                  Id = $"Review Author Id {i}",
                  UserName = $"Author {i}"
              },


          })
            .ToList();

    }
}
