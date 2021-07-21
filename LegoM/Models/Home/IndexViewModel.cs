namespace LegoM.Models.Home
{
   using LegoM.Models.Products;
   using System.Collections.Generic;

   public class IndexViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalUsers { get; set; }

        public string SearchTerm { get; set; }

        public List<ProductListingViewModel> Products {get;set;}
    }
}
