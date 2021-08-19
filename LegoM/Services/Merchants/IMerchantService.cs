namespace LegoM.Services.Merchants
{
   public interface IMerchantService
    {
        void Create(
            string userId,
            string name,
            string telephoneNumber
            );

            

        public bool IsUserMerchant(string userId);

        string IdByUser(string userId);

        string TelephoneNumberByUser(string userId);
    }
}
