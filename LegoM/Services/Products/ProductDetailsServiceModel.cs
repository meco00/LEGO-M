namespace LegoM.Services.Products
{
    public class ProductDetailsServiceModel:ProductServiceModel
    {
        public string Description { get; init; }

        public string MerchantId { get; init; }

        public string MerchantName{ get; init; }

        public string UserId{ get; init; }
    }
}
