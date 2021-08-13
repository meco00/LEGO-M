namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
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
                IsPublic=IsPublic,
                Content="TestContent",
                PublishedOn=new DateTime(1,1,1),
                Product = new Product
                {
                   
                    Title="Test",
                    Price=5,
                    ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New,
                   

                    IsPublic=true
                },           

                User=new User 
                { 
                   
                    FullName ="TestName"
                },

             

            };


        //public string Content { get; set; }

        //public string UserName { get; set; }

        //public string QuestionId { get; set; }

        //public string PublishedOn { get; set; }

        //public bool IsPublic { get; set; }


        //public int Id { get; set; }

        //public string Content { get; init; }

        //public string UserName { get; init; }

        //public string PublishedOn { get; init; }

        //public string ProductId { get; init; }

        //public string ProductTitle { get; init; }

        //public string ProductImage { get; init; }

        //public string ProductPrice { get; init; }

        //public int ProductCondition { get; init; }

        //public int AnswersCount { get; init; }

        //public IEnumerable<AnswerServiceModel> Answers { get; set; }

        //public bool IsPublic { get; init; }





        //public string ProductImage { get; init; }


    }
}
