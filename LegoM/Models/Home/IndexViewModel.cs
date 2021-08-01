namespace LegoM.Models.Home
{
   
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;

   public class IndexViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalUsers { get; set; }

        public int TotalProductsSold{ get; set; }

        public IList<ProductServiceModel> Products {get;set;}
    }
}
