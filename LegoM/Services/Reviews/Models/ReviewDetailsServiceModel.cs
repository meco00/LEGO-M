namespace LegoM.Services.Reviews.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewDetailsServiceModel:IReviewModel
    {
        public string Title { get; set; }

        public int Rating { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public string PublishedOn { get; set; }

        public string ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImage { get; set; }

        public string ProductPrice { get; set; }


        
    }
}
