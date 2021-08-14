namespace LegoM.Test.Pipeline.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using LegoM.Services.Reviews.Models;
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Data.Models;

    using ReviewsController = LegoM.Areas.Admin.Controllers.ReviewsController;
    
    using static Data.Reviews;

    public class ReviewsPipelineTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
          => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                  .WithPath("/Admin/Reviews/All")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
              .To<ReviewsController>(c => c.All())
              .Which(controller => controller
                  .WithData(GetReviews()))
              .ShouldReturn()
              .View(view => view
                 .WithModelOfType<List<ReviewServiceModel>>()
               .Passing(model => model.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeReviewAndRedirectToAll()
         => MyPipeline
               .Configuration()
                .ShouldMap(request => request
                 .WithPath($"/Admin/Reviews/ChangeVisibility/{1}")
                  .WithUser(new[] { AdminConstants.AdministratorRoleName })
                  .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.ChangeVisibility(1))
                .Which(controller => controller
                 .WithData(GetReviews(1)))
                .ShouldHave()
                 .Data(data => data
                      .WithSet<Review>(set =>
                      {
                          var review = set.FirstOrDefault(r => !r.IsPublic);

                          review.Should().NotBeNull();
                          review.Title.Should().NotBeNull();
                          review.Title.Should().Be(Data.Reviews.DEFAULT_TITLE);
                      }))
                  .AndAlso()
                  .ShouldReturn()
                  .Redirect(redirect => redirect
                     .To<ReviewsController>(c => c.All()));
    }
}
