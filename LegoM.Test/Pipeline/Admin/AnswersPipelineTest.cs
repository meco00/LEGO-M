﻿namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Areas.Admin.Models.Answers;
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
                 .To<AnswersController>(c => c.All(With.Default<AnswersQueryModel>()))
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
                        .To<AnswersController>(c => c.All(With.Default<AnswersQueryModel>())));


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
                    .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                    .Data(data=>data.WithSet<Answer>(set=>
                    {
                        set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                        set.Should().BeEmpty();

                    }))
                     .AndAlso()
                     .ShouldReturn()
                     .Redirect(redirect => redirect
                        .To<AnswersController>(c => c.All(With.Default<AnswersQueryModel>())));

        [Fact]
        public void DeleteShouldReturnNotFoundWhenAnswerIdIsInvalid()
           => MyPipeline
              .Configuration()
               .ShouldMap(request => request
                   .WithPath($"/Admin/Answers/Delete/{1}")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                      .To<AnswersController>(c => c.Delete(1))
                    .Which()
                    .ShouldReturn()
                    .NotFound();

    }



}
