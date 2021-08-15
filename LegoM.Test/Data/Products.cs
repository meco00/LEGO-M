namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    using static DataConstants;

    public static class Products
    {
        public static Product GetProduct(
            string id=TestId,
            bool IsDeleted = false,
            bool IsPublic=true)
        {
            return new Product
            {
                Id = id,
                IsPublic = IsPublic,
                IsDeleted = IsDeleted
            };
        }

        public static List<Product> GetProducts(int count=5,bool IsDeleted=false,bool sameUser=true)     
        {
              var merchant = new Merchant
              {
                Name = TestUser.Username,
                UserId = TestUser.Identifier,
              };

            var products = Enumerable
             .Range(1, count)
             .Select(i => new Product
             {
                 IsPublic = IsDeleted ? false : true,
                 IsDeleted = IsDeleted ? true : false,
                 DeletedOn = IsDeleted ? new System.DateTime(1, 1, 1) : null,
                 Merchant = sameUser ? merchant : new Merchant
                  {
                      Id = $"Author Id {i}",
                      Name = $"Author {i}"
                  },

             })
             .ToList();


            return products;

         }

        
    } 
}
