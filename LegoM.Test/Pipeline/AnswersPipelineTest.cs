namespace LegoM.Test.Pipeline
{
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Answers;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Questions;

    public class AnswersPipelineTest
    {
        public string Information = GetInformation();

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
         =>MyPipeline
               .Configuration()
                .ShouldMap(request=>request
                   .WithPath($"/Answers/Add/{1}/{Information}")
                .WithUser()
                .WithAntiForgeryToken())
                .To<AnswersController>(c=>c.Add(1,Information))
               .Which(controller=>controller.WithData(GetQuestion())
               .ShouldHave()
            .ActionAttributes(attributes => attributes
                 .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View());

        //ТODO:This test is not finished

        [Theory]
        [InlineData(1,"ContentTest")]
        [InlineData(2,"ContentTest2")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectToViewWithCorrectModel(
            int questionId,
            string content)
            => MyPipeline
               .Configuration()
                .ShouldMap(request => request.WithPath($"/Answers/Add/{questionId}")
                   .WithUser()
                    .WithAntiForgeryToken()
                   .WithMethod(HttpMethod.Post)
                   .WithFormFields(new
                   {
                       Content = content,

                   }))
                  .To<AnswersController>(c => c.Add(questionId, new AnswerFormModel
                  {
                      Content = content
                  }))
                 .Which(controller => controller.WithData(GetQuestion(questionId)))
                  .ShouldHave()
                  .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                   .ValidModelState()
                   .AndAlso()
                    .ShouldHave()
                     .Data(data => data.WithSet<Answer>(answers =>
                                        answers.Any(x => x.Content == content
                                        && x.QuestionId == questionId)))
                        .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                  .Redirect(redirect=>redirect
                          .To<QuestionsController>(c=>c
                          .Details(questionId,Information)));




        








    }
}
