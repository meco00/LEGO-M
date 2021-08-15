namespace LegoM.Test.Routing
{
    using LegoM.Controllers;
    using LegoM.Models.Reviews;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.DataConstants;

    public class ReviewsRoutingTest
    {
        [Fact]
        public void PostAddShouldBeMapped()
            => MyRouting
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Reviews/Add/{TestId}")
                     .WithMethod(HttpMethod.Post))
                 .To<ReviewsController>(c => c
                 .Add(TestId, With.Any<ReviewFormModel>()));

        [Fact]
        public void PostEditShoulBeMapped()
            =>  MyRouting
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Reviews/Edit/{1}")
                     .WithMethod(HttpMethod.Post))
                 .To<ReviewsController>(c => c
                 .Edit(1, With.Any<ReviewFormModel>()));

        [Fact]
        public void PostDeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Reviews/Delete/{1}")
                     .WithMethod(HttpMethod.Post))
                 .To<ReviewsController>(c => c
                 .Delete(1, With.Any<ReviewDeleteFormModel>()));
    }
}
