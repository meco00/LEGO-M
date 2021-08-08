namespace LegoM.Infrastructure
{
    using LegoM.Services.Reviews.Models;
    using System;

    public static class ModelsExtensions
    {

        public static string GetInformation(this IReviewModel model)
        => String.Concat(model.Title + "-" + model.Rating + "-" + model.PublishedOn);
    }
}
