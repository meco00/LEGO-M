namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Products;
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
        [InlineData(1,3)]
        public void DetailsShouldReturnCorrectModelAndView(
            int questionId,
            int answersCount)
            => MyRouting
                .Configuration()
                  .ShouldMap(request => request
                   .WithPath($"/Questions/Details/{questionId}/{Information}")
                .WithUser()
                .WithAntiForgeryToken())
                  .To<QuestionsController>(c => c.Details(questionId, Information))
               .Which(controller => controller
                     .WithData(GetQuestions(questionId))
                     .AndAlso().
                      WithData(GetAnswers(answersCount))
               .ShouldReturn()
               .View(view=>view.WithModelOfType<QuestionDetailsWithAnswersModel>()
                    .Passing(model=>
                    {
                        model.Should().NotBeNull();
                        model.Answers.Should().HaveCount(answersCount);                    
                        model.Question.Id.Should().Be(questionId);
                        model.Question.Content.Should().Be($"Question Content {questionId}");

                    }
                    )));

        [Fact]
        public void DetailsShouldReturnNotFoundWhenProductDoesNotExists()
           => MyRouting
               .Configuration()
                 .ShouldMap(request => request
                  .WithPath($"/Questions/Details/{1}/{Information}"))
                 .To<QuestionsController>(c => c.Details(1, Information))
              .Which()
              .ShouldReturn()
              .NotFound();


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

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnBadRequestWhenProductIsNotPublic()
           => MyPipeline
                 .Configuration()
                  .ShouldMap(request => request.WithPath($"/Questions/Add/{TestId}")
                     .WithUser())
                   .To<QuestionsController>(c => c.Add(TestId))
                   .Which(controller => controller
                         .WithData(GetProduct(IsPublic: false)))
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                    .AndAlso()
                    .ShouldReturn()
                     .BadRequest();


        [Theory]
        [InlineData("MyTestContent", TestId)]
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
                                               x.ProductId == productId &&
                                               x.UserId == TestUser.Identifier)))
                              .AndAlso()
                               .ShouldReturn()
                                 .Redirect(redirect =>
                                           redirect
                                           .To<ProductsController>(c =>
                                           c.Details(productId,With.Any<ProductsDetailsQueryModel>())));
        [Fact]
        public void PostAddShouldBeForAuthorizedUsersAndReturnBadRequestWhenProductIsNotPublic()
           => MyRouting
               .Configuration()
                .ShouldMap(request => request
                       .WithPath($"/Questions/Add/{TestId}")
                      .WithMethod(HttpMethod.Post)
                      .WithFormFields(new
                      {
                          Content = "TestContent"
                      })
                      .WithUser()
                      .WithAntiForgeryToken())
                      .To<QuestionsController>(c => c.Add(TestId, new QuestionFormModel
                      {
                          Content = "TestContent"
                      }))
                      .Which(controller => controller.WithData(GetProduct(IsPublic: false)))
                      .ShouldHave()
                       .ActionAttributes(attributes => attributes
                             .RestrictingForAuthorizedRequests()
                             .RestrictingForHttpMethod(HttpMethod.Post))
                     .AndAlso()
                     .ShouldReturn()
                      .BadRequest();

      



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


        [Fact]
        public void DeleteShouldReturnBadRequestWhenQuestionIsNotOfUser()
     => MyPipeline
          .Configuration()
          .ShouldMap(request => request.WithPath($"/Questions/Delete/{2}")
          .WithUser())
          .To<QuestionsController>(c => c.Delete(2))
          .Which(controller => controller.WithData(GetQuestions(1)))
          .ShouldHave()
           .ActionAttributes(attributes => attributes
                       .RestrictingForAuthorizedRequests())
             .AndAlso()
             .ShouldReturn()
             .BadRequest();


        [Fact]
        public void DeleteShouldReturnNotFoundWhenQuestionDoesNotExists()
   => MyPipeline
        .Configuration()
        .ShouldMap(request => request.WithPath($"/Questions/Delete/{2}")
        .WithUser(new[] { AdminConstants.AdministratorRoleName}))
        .To<QuestionsController>(c => c.Delete(2))
        .Which(controller => controller.WithData(GetQuestions(1)))
        .ShouldHave()
         .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .NotFound();

    }
}
