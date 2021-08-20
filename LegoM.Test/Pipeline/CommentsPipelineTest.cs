namespace LegoM.Test.Pipeline
{
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Comments;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    using static Data.Reviews;

    public class CommentsPipelineTest
    {
        public  string Information = GetInformation();

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithPath($"/Comments/Add/{1}/{Information}")
                .WithUser())
                .To<CommentsController>(c => c.Add(1, Information))
                .Which(controller => controller.WithData(GetReviews(1)))
                .ShouldHave()
                 .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .View();


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnNotFoundWhenReviewDoesNotExists()
           => MyPipeline
               .Configuration()
               .ShouldMap(request => request.WithPath($"/Comments/Add/{1}/{Information}")
               .WithUser())
               .To<CommentsController>(c => c.Add(1, Information))
               .Which()
               .ShouldHave()
                .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();


        [Theory]
        [InlineData(1, "TestContent")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnCorrectDataAndModelAndReturnRedirectToView(
            int reviewId,
            string content)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithPath($"/Comments/Add/{reviewId}/{Information}")
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new
                {
                    Content = "TestContent"
                })
                .WithUser()
                .WithAntiForgeryToken())
            .To<CommentsController>(c => c.Add(reviewId, Information, new CommentFormModel
            {
                Content = content
            }))
            .Which(controller => controller.WithData(GetReviews(1)))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
            .ValidModelState()
            .Data(data => data.WithSet<Comment>(set => set
                           .Any(x =>
                           x.ReviewId == reviewId &&
                           x.UserId == TestUser.Identifier &&
                           x.Content == content)))
            .TempData(tempData => tempData
                     .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                     .To<ReviewsController>(c => c
                     .Details(reviewId, Information)));

        [Theory]
        [InlineData(1, "TestContent")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnNotFoundWhenReviewDoesNotExists(
            int reviewId,
            string content)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request.WithPath($"/Comments/Add/{reviewId}/{Information}")
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new
                {
                    Content = "TestContent"
                })
                .WithUser()
                .WithAntiForgeryToken())
            .To<CommentsController>(c => c.Add(reviewId, Information, new CommentFormModel
            {
                Content = content
            }))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .NotFound();
    }
}
