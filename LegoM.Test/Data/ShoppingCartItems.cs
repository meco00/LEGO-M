namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public static class ShoppingCartItems
    {
        public static IEnumerable<ShoppingCartItem> GetShoppingCartItems(int count = 5,
            byte cartQuantity=1,
            byte productQuantity=5,
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
                   Quantity=cartQuantity,
                   User = sameUser ? user : new User
                   {
                       Id = $"ShoppingCart Author Id {i}",
                       UserName = $"Author {i}"
                   },
                   Product = new Product
                   {
                       Title = "Test",
                       ProductCondition = LegoM.Data.Models.Enums.ProductCondition.New,
                       Quantity=productQuantity
                   },
                   ProductId = "TestId"
               })
               .ToList();


            return cartItems;






        }
    }


}
