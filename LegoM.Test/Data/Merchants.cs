namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;

    public class Merchants
    {
        public const string TelephoneNumber = "0888888888";

        public static Trader GetMerchant(string telephoneNumber = TelephoneNumber, bool sameUser = true)
        {
            var user = new User
            {
                FullName = TestUser.Identifier,
                UserName = TestUser.Identifier
            };

            var merchant = new Trader()
            {
                User = sameUser ? user : new User
                {
                    Id = "DifferentId",
                    UserName = "DifferentName"
                },
                UserId = sameUser ? TestUser.Identifier : "DifferentId",
                TelephoneNumber = telephoneNumber

            };

            return merchant;
        }
    }
}
