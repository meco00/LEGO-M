namespace LegoM.Infrastructure
{
    using LegoM.Data.Models;
    using LegoM.Services.Favourites.Models;
    using LegoM.Services.Questions.Models;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Linq;

    public static class ModelsExtensions
    {

        public static string GetInformation(this IReviewModel model)
        => String.Concat(model.Title + "-" + model.Rating + "-" + model.PublishedOn);

        public static string GetInformation(this IQuestionModel model)
        => String.Concat(model.ProductCondition + "-" + model.PublishedOn + "-" + model.IsPublic.ToString());

     

    }
}
