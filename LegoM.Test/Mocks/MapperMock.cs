namespace LegoM.Test.Mocks
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Questions.Models;
    using Moq;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static Data.Questions;

   public class MapperMock
    {
        public static IMapper Create
        {
            get
            {
                var moq = new Mock<IMapper>();


                moq.Setup(m => m.Map<Question, QuestionDetailsServiceModel>(GetQuestion(1, true))).Returns(new QuestionDetailsServiceModel
                {
                    Id=1,
                    Content = "Test",
                    UserName = TestUser.Username,
                    ProductPrice="5.00",
                    ProductTitle= "Test",
                    ProductCondition =2

                }) ;

                return moq.Object;

                //Id = id,
                
                //Content = "Test",
                //PublishedOn = new DateTime(1, 1, 1),
                //Product = new Product
                //{

                //    Title = "Test",
                //    Price = 5,
                //    ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New,

                //    Images = new List<ProductImage>()

                //},           

                //User = new User
                //{
                //    Id = TestUser.Identifier,
                //    FullName = TestUser.Username
                //},

            }
        }

    }
}
