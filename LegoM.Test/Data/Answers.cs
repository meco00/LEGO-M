namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    using static DataConstants;

    public static class Answers
    {      
        public static IEnumerable<Answer> GetAnswers(int count=5, int questionId = 1,bool IsPublic=true)
       => Enumerable.Range(0, count).Select(i => new Answer()
       {
           User = new User
           {
               FullName = TestUser.Username
           },
           QuestionId=questionId,
           IsPublic=IsPublic
         
       });


        public static Answer GetAnswer(int id = 1, bool IsPublic = true)
            => new()
            {
                Id = id,
                IsPublic = IsPublic
            };

    }

}
