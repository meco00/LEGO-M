namespace LegoM.Models.Reviews
{
    using LegoM.Services.Comments.Models;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewDetailsWithCommentsModel
    {
        public ReviewDetailsServiceModel Review { get; init; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }
    }
}
