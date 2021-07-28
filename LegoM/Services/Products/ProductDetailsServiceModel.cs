namespace LegoM.Services.Products
{
  using System.Collections.Generic;

    public class ProductDetailsServiceModel:ProductServiceModel
    {
        public byte Quantity { get; init; }

        public string Description { get; init; }

        public string Delivery { get; init; }

        public string MerchantId { get; init; }

        public string MerchantName{ get; init; }

        public string UserId{ get; init; }

        public IEnumerable<string> SubCategoriesIds { get; init; }
    }
}
