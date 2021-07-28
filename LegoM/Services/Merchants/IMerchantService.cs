namespace LegoM.Services.Merchants
{
   public interface IMerchantService
    {
        public bool IsMerchant(string userId);

        string IdByUser(string userId);
    }
}
