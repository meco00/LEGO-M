namespace LegoM.Services.Reviews.Models
{
    using LegoM.Services.Comments.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewDetailsServiceModel:IReviewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Rating { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string PublishedOn { get; set; }

        public string ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImage { get; set; }

        public string ProductPrice { get; set; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }



    }
}
