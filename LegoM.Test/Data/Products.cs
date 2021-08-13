namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    using static DataConstants;

    public static class Products
    {
       

        public static Product GetProduct(string id=TestId,bool isPublic = true)
        {
            return new Product
            {
                Id = id,
                IsPublic = isPublic
            };
        }

        public static Product GetDeadProduct(string id=TestId)
        {
            return new Product
            {
                Id = id,
                IsDeleted = true
            };
        }



        public static IEnumerable<Product> TenPublicProducts()
         => Enumerable.Range(0, 10).Select(i => new Product()
          {
            IsPublic = true
          });

        public static IEnumerable<Product> TenDeletedProducts()
         => Enumerable.Range(0, 10).Select(i => new Product()
         {
             IsDeleted = true,
             DeletedOn = new System.DateTime(1, 1, 1)

         });
    } 
}
