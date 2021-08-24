namespace LegoM.Test.Controllers
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using LegoM.Models.Reviews;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    using static Data.Products;
    using static Data.Reviews;

    public  class ReviewsControllerTest
    {
        [Theory]
        [InlineData(ReviewType.Excellent, Title, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
          ReviewType rating,
          string title,
          string content
            )
          => MyController<ReviewsController>
              .Instance(controller => controller
                        .WithUser()
                        .WithData(GetProduct()))
               .Calling(c => c.Add(ProductTestId, new ReviewFormModel
               {
                   Rating = rating,
                   Title = title,
                   Content = content

               }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Review>(set => set
                             .Any(x =>
                             x.Rating == rating &&
                             x.Title == title &&
                             x.Content == content &&
                             x.UserId == TestUser.Identifier)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                  .Redirect(redirect => redirect
                        .To<ProductsController>(c => c
                         .Details(ProductTestId, With.Any<ProductsDetailsQueryModel>())));

        [Theory]
        [InlineData(ReviewType.Excellent, Title, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnBadRequestWhenProductIsNotPublic(
        ReviewType rating,
        string title,
        string content
          )
        => MyController<ReviewsController>
            .Instance(controller => controller
                      .WithUser()
                      .WithData(GetProduct(IsPublic: false)))
             .Calling(c => c.Add(ProductTestId, new ReviewFormModel
             {
                 Rating = rating,
                 Title = title,
                 Content = content

             }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests()
                  .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldReturn()
            .BadRequest();  


        [Theory]
        [InlineData(ReviewType.Excellent,Title, TestContent)]
        public void PostEditShouldBeForAuthorizedUsersAndReturnCorrectDataAndModelAndRedirectTo(
            ReviewType rating,
            string title,
            string content)
          => MyController<ReviewsController>
               .Instance(controller => controller
                        .WithUser()
                        .WithData(GetReviews(1)))
               .Calling(c => c.Edit(1, new ReviewFormModel
               {
                   Rating = rating,
                   Title = title,
                   Content = content

               }))
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests()
                      .RestrictingForHttpMethod(HttpMethod.Post))
               .ValidModelState()
               .Data(data => data.WithSet<Review>(set => set.Any(x =>
                                     x.Rating == rating &&
                                    x.Title == title &&
                                    x.Content == content)))
               .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                       .To<ReviewsController>(c => c
                       .Mine()));

        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndReturnBadRequestWhenReviewIsNotOfUser()
        => MyController<ReviewsController>
             .Instance(controller => controller
                      .WithUser()
                      .WithData(GetReviews(1, sameUser: false)))
             .Calling(c => c.Edit(1, With.Any<ReviewFormModel>()))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldReturn()
            .BadRequest();

        [Fact]
        public void PostEditShouldBeForAuthorizedUsersAndWhenUserIsAdminReturnBadRequestWhenReviewDoesNotExists()
        => MyController<ReviewsController>
             .Instance(controller => controller
                      .WithUser(new[] { AdminConstants.AdministratorRoleName}))
             .Calling(c => c.Edit(1, new ReviewFormModel
             {
                 Rating = ReviewType.Excellent,
                 Title = Title,
                 Content = TestContent,

             }))
             .ShouldHave()
             .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
             .ValidModelState()
             .AndAlso()
            .ShouldReturn()
            .NotFound();



        [Fact]
        public void PostDeleteShouldDeleteReviewAndReturnCorrectDataAndRedirectToMine()
            => MyController<ReviewsController>
               .Instance(controller => controller
                        .WithUser()
                        .WithData(GetReviews(1)))
                 .Calling(c => c.Delete(1, new ReviewDeleteFormModel
                 {
                   SureToDelete=true

                 }))
              .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests()
                                  .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                   .Data(data => data.WithSet<Review>(set =>
                   {
                       set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                       set.Should().BeEmpty();
                   }))
               .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                       .To<ReviewsController>(c => c
                       .Mine()));
    }
}
