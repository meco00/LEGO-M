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
                IsPublic=IsPublic
            };
    }
}
