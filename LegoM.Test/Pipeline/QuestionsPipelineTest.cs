﻿namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Questions;
    using LegoM.Services.Questions.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;


    using static Data.Answers;
    using static Data.DataConstants;
    using static Data.Products;
    using static Data.Questions;

    public class QuestionsPipelineTest
    {
        public string Information = GetInformation();

        [Theory]
        [InlineData(3)]
        public void DetailsShouldReturnCorrectModelAndView(
            
            int answersCount)
            => MyRouting
                .Configuration()
                  .ShouldMap(request => request
                   .WithPath($"/Questions/Details/{1}/{Information}")
                .WithUser()
                .WithAntiForgeryToken())
                  .To<QuestionsController>(c => c.Details(1, Information))
               .Which(controller => controller
                     .WithData(GetQuestions(1))
                     .AndAlso().
                      WithData(GetAnswers(1,answersCount))
               .ShouldReturn()
               .View(view=>view.WithModelOfType<QuestionDetailsAndAnswersModel>()
                    .Passing(model=>
                    {
                        model.Answers.Should().HaveCount(answersCount);                    
                        model.Question.Id.Should().Be(1);

                    }
                    )));


        [Fact]
        public void MineShouldBeForAuthorizedUsersAndReturnViewWithCorrectDataAndModel()
            => MyRouting
                   .Configuration()
                    .ShouldMap(request => request
                        .WithPath("/Questions/Mine")
                        .WithUser()
                        .WithAntiForgeryToken())
                    .To<QuestionsController>(c => c.Mine())
                    .Which(controller => controller.WithData(GetQuestions()))
                    .ShouldHave()
                    .ActionAttributes(attributes=>attributes
                                   .RestrictingForAuthorizedRequests())
                    .AndAlso()
                    .ShouldReturn()
                    .View(view => view
                        .WithModelOfType<List<QuestionListingServiceModel>>()
                         .Passing(model => model.Should().HaveCount(5)));


        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
            => MyPipeline
                  .Configuration()
                   .ShouldMap(request => request.WithPath($"/Questions/Add/{TestId}")
                      .WithUser()
                      .WithAntiForgeryToken())
                    .To<QuestionsController>(c => c.Add(TestId))
                    .Which(controller => controller
                          .WithData(GetProduct()))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                           .RestrictingForAuthorizedRequests())
                     .AndAlso()
                     .ShouldReturn()
                      .View();


        [Theory]
        [InlineData("MyTestContent",TestId)]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectToWithCorrectDataAndModel(
            string content,
            string productId)
            => MyRouting
                .Configuration()
                 .ShouldMap(request => request
                        .WithPath($"/Questions/Add/{productId}")
                       .WithMethod(HttpMethod.Post)
                       .WithFormFields(new
                       {
                           Content = content
                       })
                       .WithUser()
                       .WithAntiForgeryToken())
                       .To<QuestionsController>(c => c.Add(productId, new QuestionFormModel
                       {
                           Content = content
                       }))
                       .Which(controller => controller.WithData(GetProduct()))
                       .ShouldHave()
                        .ActionAttributes(attributes => attributes
                              .RestrictingForAuthorizedRequests()
                              .RestrictingForHttpMethod(HttpMethod.Post))
                        .ValidModelState()
                         .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                             .Data(data => data.WithSet<Question>(set => set
                                     .Any(x => x.Content == content &&
                                               x.ProductId == productId&&
                                               x.UserId==TestUser.Identifier)))
                              .AndAlso()
                               .ShouldReturn()
                                 .Redirect(redirect =>
                                           redirect
                                           .To<ProductsController>(c =>
                                           c.Details(productId)));


        [Theory]
        [InlineData(1)]
        public void DeleteShouldDeleteQuestionAndRedirectToMine(
            int questionId)
            => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request.WithPath($"/Questions/Delete/{questionId}")
                 .WithUser()
                 .WithAntiForgeryToken())
                 .To<QuestionsController>(c => c.Delete(questionId))
                 .Which(controller => controller.WithData(GetQuestions(1)))
                 .ShouldHave()
                  .ActionAttributes(attributes => attributes
                              .RestrictingForAuthorizedRequests())
                    .Data(data => data.WithSet<Question>(set =>
                    {
                        set.FirstOrDefault(x => x.Id == questionId).Should().BeNull();
                        set.Should().BeEmpty();
                    }))
                    .TempData(tempData => tempData
                             .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                    .AndAlso()
                    .ShouldReturn()
                     .Redirect(redirect => redirect
                               .To<QuestionsController>(c => c.Mine()));


    }
}
