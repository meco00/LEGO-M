namespace LegoM.Services.Merchants
{
   public interface IMerchantService
    {
        public bool IsMerchant(string userId);

        string GetMerchantId(string userId);
    }
}
