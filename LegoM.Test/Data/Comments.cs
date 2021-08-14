﻿namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public static class Comments
    {
        public static IEnumerable<Comment> GetComments(int count=5)
         => Enumerable.Range(0, count).Select(i => new Comment()
         {
             User = new User
             {
                 FullName = "TestName"
             },
            

             
         });

        public static Comment GetComment(int id = 1, bool IsPublic = true)
          => new Comment
          {
              Id = id,
              IsPublic = IsPublic
          };

    }
}