namespace LegoM.Test.Mocks
{
    using LegoM.Services.Questions;
    using Moq;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static Data.Questions;

   public class QuestionServiceMock
    {
        public static IQuestionsService Create
        {
            get
            {
                var serviceMock = new Mock<IQuestionsService>();

                serviceMock.Setup(s => s.Details(1)).Returns(new Services.Questions.Models.QuestionDetailsServiceModel
                {
                    Id = 1,
                    Content = "TestContent",
                    PublishedOn= TestDateTime.ToString("dd MMM yyy"),
                    UserName = TestUser.Username,
                    ProductPrice = "5.00",
                    ProductTitle = "Test",
                    ProductCondition = 2
                });




                return serviceMock.Object;
            }
        }
    }
}
