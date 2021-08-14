namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    using static DataConstants;

    public static class Answers
    {

        public static IEnumerable<Answer> GetAnswers(int questionId=1,int count=5)
       => Enumerable.Range(0, count).Select(i => new Answer()
       {
           User = new User
           {
               FullName = "TestName"
           },
           QuestionId=questionId,
           IsPublic=true
         
       });


        public static Answer GetAnswer(int id = 1, bool IsPublic = true)
            => new Answer
            {
                Id = id,
                IsPublic = IsPublic
            };

    }

}
