namespace LegoM.Infrastructure
{
    using LegoM.Controllers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    public static class EndPointRouteBuilderExtensions
    {
        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
        => endpoints.MapControllerRoute(
            name: "Areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );


        public static void MapReviewsDetailsRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute
                (
                  name: "Review Details",
                  pattern: "/Reviews/Details/{id}/{information}",
                  defaults: new
                  {
                      controller = typeof(ReviewsController).GetControllerName(),
                      action = nameof(ReviewsController.Details)
                  }
                );

        public static void MapQuestionsDetailsRoute(this IEndpointRouteBuilder endpoints)
           => endpoints.MapControllerRoute
               (
                 name: "Question Details",
                 pattern: "/Questions/Details/{id}/{information}",
                 defaults: new
                 {
                     controller = typeof(QuestionsController).GetControllerName(),
                     action = nameof(QuestionsController.Details)
                 }
               );

        public static void MapAnswersAddRoute(this IEndpointRouteBuilder endpoints)
        => endpoints.MapControllerRoute
            (
              name: "Answers Add",
              pattern: "/Answers/Add/{id}/{information}",
              defaults: new
              {
                  controller = typeof(AnswersController).GetControllerName(),
                  action = nameof(AnswersController.Add)
              }
            );

        public static void MapCommentsAddRoute(this IEndpointRouteBuilder endpoints)
       => endpoints.MapControllerRoute
           (
             name: "Comments Add",
             pattern: "/Comments/Add/{id}/{information}",
             defaults: new
             {
                 controller = typeof(CommentsController).GetControllerName(),
                 action = nameof(CommentsController.Add)
             }
           );


    }
}
