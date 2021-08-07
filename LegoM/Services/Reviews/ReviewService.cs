namespace LegoM.Services.Reviews
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
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

        public void Create(string productId,string userId, ReviewType rating, string content, string title)
        {
            if (title==null)
            {
                title = rating.ToString();
            }



            var review = new Review
            {
                Rating = rating,
                Content = content,
                Title = title,
                ProductId = productId,
                UserId = userId

            };


            this.data.Add(review);

            this.data.SaveChanges();

            
        }
    }
}
