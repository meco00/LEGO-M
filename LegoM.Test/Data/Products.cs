namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
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

        public static IEnumerable<Product> GetProducts(int count=5,bool IsDeleted=false)
         => Enumerable.Range(0, count).Select(i => new Product()
          {
            IsPublic = IsDeleted ? false:true,
            IsDeleted = IsDeleted ? true:false,
            DeletedOn=IsDeleted ? new System.DateTime(1, 1, 1) :null

          });

        
    } 
}
