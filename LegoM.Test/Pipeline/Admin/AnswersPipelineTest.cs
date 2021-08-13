namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Data.Models;
    using LegoM.Services.Answers.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static Data.Answers;
    using AnswersController = LegoM.Areas.Admin.Controllers.AnswersController;


    public class AnswersPipelineTest
    {

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request.WithPath("/Admin/Answers/All")
                 .WithUser(new[] { AdminConstants.AdministratorRoleName })
                 .WithAntiForgeryToken())
                 .To<AnswersController>(c => c.All())
                  .Which(controller => controller
                     .WithData(GetAnswers()))
                   .ShouldReturn()
                   .View(view => view
                   .WithModelOfType<List<AnswerServiceModel>>()
                   .Passing(model => model.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeAnswerAndRedirectToAll()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request
                    .WithPath($"/Admin/Answers/ChangeVisibility/{1}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                   .To<AnswersController>(c => c.ChangeVisibility(1))
                   .Which(controller => controller
                    .WithData(GetAnswer()))
                   .ShouldHave()
                    .Data(data => data
                         .WithSet<Answer>(set => set
                              .Any(x => x.Id == 1 && !x.IsPublic)))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<AnswersController>(c => c.All()));


        [Fact]
        public void DeleteShouldDeleteAnswerAndRedirectToAll()
            =>MyPipeline
               .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Admin/Answers/Delete/{1}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName })
                     .WithAntiForgeryToken())
                       .To<AnswersController>(c=>c.Delete(1))
                     .Which(controller=>controller.WithData(GetAnswer()))
                    .ShouldHave()
                    .Data(data=>data.WithSet<Answer>(set=>
                    {
                        set.Should().NotContain(GetAnswer());
                        set.Should().BeEmpty();

                        }))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<AnswersController>(c => c.All()));

    }



}
