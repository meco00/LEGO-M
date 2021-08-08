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

    }
}
