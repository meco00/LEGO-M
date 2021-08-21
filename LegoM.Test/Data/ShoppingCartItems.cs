namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ShoppingCartItems
    {
        public static IEnumerable<ShoppingCartItem> GetShoppingCartItems(int count = 5,
            bool isPublic = true,
            bool sameUser = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };


            var cartItems = Enumerable
               .Range(1, count)
               .Select(i => new ShoppingCartItem
               {
                   Id = i,
                   Quantity=1,
                   User = sameUser ? user : new User
                   {
                       Id = $"ShoppingCart Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Product = new Product
                   {
                       Title = "Test",
                       ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New,
                       Quantity=5
                   },
                   ProductId = "TestId"
               })
               .ToList();


            return cartItems;






        }
    }


}
