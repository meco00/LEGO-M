namespace LegoM.Test.Pipeline
{
    using LegoM.Controllers;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using LegoM.Models.Reviews;
    using FluentAssertions;


    using static Data.Reviews;
    using static Data.Comments;
    using static Data.Products;
    using static Data.DataConstants;
    using LegoM.Data.Models.Enums;
    using LegoM.Data.Models;
    using LegoM.Services.Reviews.Models;

    public class ReviewsPipelineTest
    {
        public string Information = GetInformation();   

        [Theory]
        [InlineData(1,3)]
        public void DetailsShouldReturnViewWithCorrectDataAndModel(
            int detailsId,
            int commentsCount)
            => MyPipeline
                  .Configuration()
                  .ShouldMap(request => request
                  .WithPath($"/Reviews/Details/{detailsId}/{Information}")
                  .WithUser())
            .To<ReviewsController>(c => c.Details(detailsId, Information))
            .Which(controller => controller
                      .WithData(GetReviews(detailsId))
                      .AndAlso()
                      .WithData(GetComments(commentsCount))
            .ShouldReturn()
            .View(view => view.WithModelOfType<ReviewDetailsWithCommentsModel>()
                  .Passing(model =>
                  {
                      model.Should().NotBeNull();
                      model.Review.Should().NotBeNull();
                      model.Review.Id.Should().Be(1);
                      model.Review.Title.Should().Be(DEFAULT_TITLE);
                      model.Comments.Should().HaveCount(commentsCount);
                  })));



        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Add/{TestId}")
                    .WithUser()
                    .WithAntiForgeryToken())
                  .To<ReviewsController>(c => c.Add(TestId))
                  .Which(controller => controller
                        .WithData(GetProduct()))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                    .View();



        [Fact]
        public void MineShouldBeForAuthorizedUsersAndReturnViewWithCorrectDataAndModel()
           => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                       .WithPath("/Reviews/Mine")
                       .WithUser()
                       .WithAntiForgeryToken())
                   .To<ReviewsController>(c => c.Mine())
                   .Which(controller => controller.WithData(GetReviews()))
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view
                       .WithModelOfType<List<ReviewListingServiceModel>>()
                        .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void GetEditShouldBeForAuthorizedUsersAndReturnViewAndCorrectModelAndData()
          => MyPipeline
                  .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Edit/{1}")
                  .WithUser())
                .To<ReviewsController>(c => c.Edit(1))
                .Which(controller => controller.WithData(GetReviews(1)))
                .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View(view => view.WithModelOfType<ReviewFormModel>()
                   .Passing(model=>
                   {
                       model.Content.Should().Be(TestContent);
                       model.Rating.Should().Be(ReviewType.Excellent);
                    }));

        [Fact]
        public void GetDeleteShouldBeForAuthorizedUsersAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                 .ShouldMap(request => request.WithPath($"/Reviews/Delete/{1}")
                 .WithUser())
                 .To<ReviewsController>(c => c.Delete(1))
                 .Which(controller => controller.WithData(GetReviews(1)))
                .ShouldHave()
                   .ActionAttributes(attributes => attributes
                                  .RestrictingForAuthorizedRequests())
                   .AndAlso()
                   .ShouldReturn()
                   .View();

       

    }
}


