namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    using static Comments;

    public static class Reviews
    {


        public static IEnumerable<Review> GetReviews(int count=5)
        => Enumerable.Range(0, count).Select(i => new Review()
        {
            User = new User
            {
                FullName = "TestName"
            },
            Product=new Product
            {
                Title="Test"
            },                     
           
        });


        public static Review GetReview(int id = 1, bool IsPublic = true)
         => new Review
         {
             Id = id,
             IsPublic = IsPublic
         };

    }
}
