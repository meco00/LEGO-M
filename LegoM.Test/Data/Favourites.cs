namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    using static Data.Products;

    public static class Favourites
    {
        public static List<Favourite> GetFavourites(
            int count = 5,            
            bool sameUser = true,
            bool sameProduct = true)
        {

            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };

            var product = new Product
            {
                Id = ProductTestId,
                ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New
            };


            var favourites = Enumerable
             .Range(1, count)
             .Select(i => new Favourite
             {
                 Id = i,               
                 User = sameUser ? user : new User
                 {
                     Id = $"Author Id {i}",
                     UserName = $"Author {i}"
                 },
                 Product =  sameProduct ? product : new Product
                 {
                     ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New
                 },
             })
             .ToList();


            return favourites;

        }
    }
}
