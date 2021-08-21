namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public static class Comments
    {      
        public static IEnumerable<Comment> GetComments(int count=5,int reviewId=1, bool IsPublic = true)
          => Enumerable.Range(0, count).Select(i => new Comment() 
          {
              User = new User
              {
                  FullName = TestUser.Username
              },
              IsPublic = IsPublic,
              ReviewId=reviewId
          });



    }
}
