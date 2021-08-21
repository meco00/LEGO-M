namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Users
    {
        public const string TelephoneNumber = "0888888888";

        public static IEnumerable<User> GetUsers(int count=5)
        => Enumerable.Range(0, count).Select(i => new User()
        {
           
        });


        public static Merchant GetMerchant(string userId = TestUser.Identifier,string telephoneNumber=TelephoneNumber,bool sameUser=true)
        {
            var user = new User
            {
                FullName = TestUser.Identifier,
                UserName = TestUser.Identifier
            };

            var merchant = new Merchant() 
            {
                User = sameUser ? user : new User
                {
                    Id = "DifferentId",
                    UserName = "DifferentName"
                },
                UserId = sameUser ? TestUser.Identifier : "DifferentId",
                TelephoneNumber =telephoneNumber
                
            };

            return merchant;
        }



    }
}
