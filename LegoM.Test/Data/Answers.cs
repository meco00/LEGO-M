namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    using static DataConstants;

    public static class Answers
    {

        public static IEnumerable<Answer> TenAnswers()
       => Enumerable.Range(0, 10).Select(i => new Answer()
       {
           User = new User
           {
               FullName = "TestName"
           },
         
       });


        public static Answer GetAnswer(int id = 1, bool IsPublic = true)
            => new Answer
            {
                Id = id,
                IsPublic = IsPublic
            };

    }

}
