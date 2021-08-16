namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public static class Questions
    {
        public static DateTime TestDateTime = new DateTime(1, 1, 1);

        public static string GetInformation(int condition=2,string content="Quest" )
            => String.Concat(condition + "-" + TestDateTime.ToString("dd MMM yyy") + "-" + new string(content.Take(5).ToArray()));



        //      public static string GetInformation(this IQuestionModel model)
        //=> String.Concat(model.ProductCondition + "-" + model.PublishedOn + "-" + new string(model.Content.Take(5).ToArray()));


        public static List<Question> GetQuestions(         
            int count=5,
            bool isPublic=true,
            bool sameUser=true)

        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };


            var questions = Enumerable
               .Range(1, count)
               .Select(i => new Question
               {
                   Id = i,
                  
                   Content = $"Question Content {i}",
                   IsPublic = isPublic,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Question Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Product = new Product
                   {
                       ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New
                   },
                   
               })
               .ToList();


            return questions;



        }




        public static List<Question> GetQuestionsByProduct(string productId = "TestId", int count = 5)
          => Enumerable.Range(0, count).Select(i => new Question()
          {
              Content = "Question Content",
              IsPublic = true,
              PublishedOn = new DateTime(1, 1, 1),
              ProductId = productId,
              User =  new User
               {
                     Id = $"Question Author Id {i}",
                     UserName = $"Author {i}"
               },

          })
            .ToList();





    }
}
