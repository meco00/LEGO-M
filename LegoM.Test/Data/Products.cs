namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class Products
    {
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
