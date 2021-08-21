namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static ShoppingCartItems;

    public static class Orders
    {
        public static Order GetOrder(
             bool accomplished = false,
            bool sameUser = true,
            int cartItemsCount = 5,
            byte quantityPerItem = 1,
            byte quantityPerProduct = 5,
            int orderId=1)

        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            var order = new Order
            {
                Id = orderId,
                FullName = TestUser.Username,
                State = $"Kirdjali {orderId}",
                City = $"Momchigrad {orderId}",
                Address = $"My Addres {orderId}",
                OrderedOn = new DateTime(1, 1, 1),
                IsAccomplished = accomplished,
                User = sameUser ? user : new User
                {
                    Id = $"Author Id {orderId}",
                    UserName = $"Author {orderId}"
                },
                ShoppingCart = GetShoppingCartItems(cartItemsCount, quantityPerItem, quantityPerProduct)
                                           .ToList()
            };

            return order;

        }




        public static List<Order> GetOrders(
            int count = 5,
            bool accomplished = false,
            bool sameUser = true,
            int cartItemsCount=5,
            byte quantityPerItem=1,
            byte quantityPerProduct=5)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };


            var orders = Enumerable
             .Range(1, count)
             .Select(i => new Order
             {
                 Id = i,
                 FullName = TestUser.Username,
                 State = $"Kirdjali {i}",
                 City = $"Momchigrad {i}",
                 Address = $"My Addres {i}",
                 OrderedOn = new DateTime(1, 1, 1),
                 IsAccomplished = accomplished,
                 User = sameUser ? user : new User
                 {
                     Id = $"Author Id {i}",
                     UserName = $"Author {i}"
                 },
                

             })
             .ToList();

            return orders;
        }
    }
}


//public int Id { get; set; }

//public string FullName { get; set; }

//public string State { get; set; }

//public string City { get; set; }

//public string OrderedOn { get; set; }

//public string TotalAmount { get; set; }