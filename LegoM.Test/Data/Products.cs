namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;


    public static class Products
    {
        public const string ProductTestId = "TestId";

        public const string FirstImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Cat_img.jpg";

        public const string SecondImageUrl = "https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png";

        public const string ThirdImageUrl = "https://timesofindia.indiatimes.com/photo/67586673.cms";


        public static Product GetProduct(
            string id=ProductTestId,
            bool userSame=true,
            bool IsDeleted = false,
            bool IsPublic=true)
        {

            var user = new User 
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };


            var trader = new Trader
            {
                
                Name = TestUser.Username,               
                User= userSame ? user : new User
                {
                    Id="DifferentId",
                    UserName="DifferentName"
                },
                UserId= userSame? TestUser.Identifier : "DifferentId"
            };

            return new Product
            {
                Id = id,
                Title = "Title",
                IsPublic = IsPublic,
                IsDeleted = IsDeleted,
                Quantity=5,
                Trader = trader,
                Category = new Category
                {
                    Id = 1,
                    Name = "TestCategory",



                },
                SubCategory = new SubCategory 
                {
                    Id=1,
                    Name = "TestSubCategory",
                    Category=new Category
                    {
                        Id=2,
                        Name = "TestCategory"
                    },
                    CategoryId=2
                },
                Images=new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        ImageUrl="TestUrl"
                    }

                }

            };
        }
 

        public static List<Product> GetProducts(int count=5,bool IsDeleted=false,bool sameUser=true)     
        {
              var merchant = new Trader
              {
                Name = TestUser.Username,
                UserId = TestUser.Identifier,
              };

            var products = Enumerable
             .Range(1, count)
             .Select(i => new Product
             {
                 IsPublic = !IsDeleted ,
                 IsDeleted = IsDeleted ,
                 DeletedOn = IsDeleted ? new System.DateTime(1, 1, 1) : null,
                 Trader = sameUser ? merchant : new Trader
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
