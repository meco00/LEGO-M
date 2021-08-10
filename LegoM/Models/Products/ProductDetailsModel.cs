namespace LegoM.Models.Products
{
    using LegoM.Services.Products.Models;
    using LegoM.Services.Questions.Models;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductDetailsModel
    {
        
        public ProductDetailsServiceModel Product { get; init; }

        public IEnumerable<ProductServiceModel> SimilarProducts { get; init; }

        public ReviewsProductStatisticsServiceModel ProductReviewsStatistics { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }

        public IEnumerable<QuestionServiceModel> Questions { get; set; }

    }
}
