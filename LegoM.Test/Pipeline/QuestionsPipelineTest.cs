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
    using LegoM.Services.Questions.Models;
    using FluentAssertions;
    using LegoM.Models.Questions;


    using static Data.Answers;
    using static Data.Questions;

    public class QuestionsPipelineTest
    {
        public string Information = GetInformation();

        [Theory]
        [InlineData(1,3)]
        public void DetailsShouldReturnCorrectModelAndView(
            int id,
            int answersCount)
            => MyRouting
                .Configuration()
                  .ShouldMap(request => request
                   .WithPath($"/Questions/Details/{id}/{Information}")
                .WithUser()
                .WithAntiForgeryToken())
                  .To<QuestionsController>(c => c.Details(id, Information))
               .Which(controller => controller
                     .WithData(GetQuestion(id))
                     .AndAlso().
                      WithData(GetAnswers(id,answersCount))
               .ShouldReturn()
               .View(view=>view.WithModelOfType<QuestionDetailsAndAnswersModel>()
                    .Passing(model=>
                    {
                        model.Answers.Should().HaveCount(answersCount);                    
                        model.Question.Id.Should().Be(id);

                    }
                    )));

      
        

    }
}
