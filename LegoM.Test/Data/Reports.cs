namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

  public static  class Reports
    {
        public const string TestContent = nameof(TestContent);


        public static List<Report> GetReports(
            int count = 5,
            bool sameUser = true)    
       {

            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };


            var reports = Enumerable
               .Range(1, count)
               .Select(i => new Report
               {
                   Id = i,
                   ReportType = LegoM.Data.Models.Enums.ReportType.Sexual,
                   Content = TestContent,
                   PublishedOn = new DateTime(1, 1, 1),
                   User = sameUser ? user : new User
                   {
                       Id = $"Review Author Id {i}",
                       UserName = $"Author {i}"
                   },                 
                   ProductId = "TestId"
               })
               .ToList();


            return reports;

        }

        //public string ReportType { get; set; }

        //public string Content { get; set; }

        //public string UserName { get; set; }

        //public string ProductId { get; set; }

        //public string ProductTitle { get; set; }

        //public string PublishedOn { get; set; }


    }
}
