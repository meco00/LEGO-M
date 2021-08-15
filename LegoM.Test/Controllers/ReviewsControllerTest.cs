namespace LegoM.Test.Controllers
{
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Products;
    using static Data.Reviews;
    using static Data.DataConstants;
    using LegoM.Models.Reviews;
    using LegoM.Data.Models.Enums;

    public  class ReviewsControllerTest
    {
        public const string TestContent = nameof(TestContent);


        [Theory]
        [InlineData(ReviewType.Excellent, DEFAULT_TITLE, TestContent)]
        public void PostAddShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
          ReviewType rating,
          string title,
          string content
            )
          => MyController<ReviewsController>
              .Instance(controller => controller
                        .WithUser()
                        .WithData(GetProduct()))
               .Calling(c => c.Add(TestId,new ReviewFormModel
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
                             x.UserId==TestUser.Identifier)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                  .Redirect(redirect => redirect
                        .To<ProductsController>(c => c
                        .Details(TestId)));

        [Theory]
        [InlineData(ReviewType.Excellent,DEFAULT_TITLE, TestContent)]
        public void PostEditShouldBeForAuthorizedUsersAndReturnCorrectDataAndModelAndRedirectTo(
            ReviewType rating,
            string title,
            string content)
          => MyController<ReviewsController>
               .Instance(controller => controller
                        .WithUser()
                        .WithData(GetReviews()))
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
    }
}
