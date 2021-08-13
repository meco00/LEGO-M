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

    using static Data.Questions;

    public class QuestionsPipelineTest
    {
        public string Information = GetInformation();

        [Fact]
        public void DetailsShouldReturnCorrectModelAndView()
            => MyRouting
                .Configuration()
                  .ShouldMap(request => request
                   .WithPath($"/Questions/Details/{1}/{Information}")
                .WithUser()
                .WithAntiForgeryToken())
                  .To<QuestionsController>(c => c.Details(1, Information))
               .Which(controller => controller.WithData(GetQuestion())
               .ShouldReturn()
               .View());

        //TODO: THROWS 'Unable to cast object
        //of type 'System.Linq.Expressions.NewExpression'
        //to type 'System.Linq.Expressions.MethodCallExpression
        // when map Question to QuestionDetailsService 
        

    }
}
