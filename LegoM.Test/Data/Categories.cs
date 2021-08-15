namespace LegoM.Test.Data
{
    using LegoM.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public static class Categories
    {
        public static IEnumerable<Category> GetCategories(int count = 5)
            => Enumerable.Range(0, count).Select(i => new Category()
            {
                SubCategories = GetSubCategories(1).ToList()

            });

        public static IEnumerable<SubCategory> GetSubCategories(int count=5)
           => Enumerable.Range(0, count).Select(i => new SubCategory()
           {

           });
    }
}
