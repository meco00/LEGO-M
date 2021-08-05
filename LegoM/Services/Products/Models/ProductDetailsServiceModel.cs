namespace LegoM.Services.Products.Models
{
  using System.Collections.Generic;

    public class ProductDetailsServiceModel:ProductServiceModel
    {
        public byte Quantity { get; init; }

        public string Description { get; init; }

        public string Delivery { get; init; }

        public string MerchantId { get; init; }

        public string MerchantName{ get; init; }

        public string SecondImageUrl { get; init; }

        public string ThirdImageUrl { get; init; }

        public string UserId{ get; init; }

        public string CategoryId { get;init;}

        public string CategoryName { get; set; }

        public string SubCategoryId { get;init;}

        public string SubCategoryName { get;init;}
    }
}
