﻿namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Data.Models;
    using LegoM.Services.Questions.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    using static Data.Questions;

    using QuestionsController = LegoM.Areas.Admin.Controllers.QuestionsController;


    public class QuestionsPipelineTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewAndModel()
           => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Admin/Questions/All")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName })
                    .WithAntiForgeryToken())
               .To<QuestionsController>(c => c.All())
               .Which(controller => controller
                   .WithData(GetQuestions()))
               .ShouldReturn()
               .View(view => view
                  .WithModelOfType<List<QuestionServiceModel>>()
                .Passing(model => model.Should().NotBeEmpty()));


        [Fact]
        public void ChangeVisibilityShouldChangeQuestionAndRedirectToAll()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request
                  .WithPath($"/Admin/Questions/ChangeVisibility/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
                 .To<QuestionsController>(c => c.ChangeVisibility(1))
                 .Which(controller => controller
                  .WithData(GetQuestions(1)))
                 .ShouldHave()
                  .Data(data => data
                       .WithSet<Question>(set => set
                            .Any(x => x.Id == 1 && !x.IsPublic)))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<QuestionsController>(c => c.All()));


    }
}
