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

        public static string GetInformation(int condition=2,string content="TestContent" )
            => String.Concat(condition + "-" + TestDateTime.ToString("dd MMM yyy") + "-" + new string(content.Take(5).ToArray()));



        //      public static string GetInformation(this IQuestionModel model)
        //=> String.Concat(model.ProductCondition + "-" + model.PublishedOn + "-" + new string(model.Content.Take(5).ToArray()));


        


        public static IEnumerable<Question> GetQuestions(int count=5)
        => Enumerable.Range(0, count).Select(i => new Question()
        {
            User = new User
            {
                FullName = "TestName"
            },

            Product = new Product
            {
                ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New
            },

        });

        public static Question GetQuestion(int id = 1,bool IsPublic=true)
            => new Question
            {
                Id = id,
                
                Content="TestContent",
                PublishedOn=new DateTime(1,1,1),
                Product = new Product
                {
                   
                    Title="Test",
                    Price=5,
                    ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New,
                   
                    
                   
                },           

                User=new User 
                { 
                  
                    FullName =TestUser.Username
                },

                IsPublic=IsPublic

             

            };


       

    }
}
