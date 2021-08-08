namespace LegoM.Services.Reviews.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IReviewModel
    {
        string Title { get; }
        int Rating { get; }
        string PublishedOn { get; }
    }
}
