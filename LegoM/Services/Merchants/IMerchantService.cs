namespace LegoM.Services.Merchants
{
   public interface IMerchantService
    {
        void Create(
            string userId,
            string name,
            string telephoneNumber
            );

             //Name = merchant.Name,
             //   TelephoneNumber = merchant.TelephoneNumber,
             //   UserId = userId

        public bool IsUserMerchant(string userId);

        string IdByUser(string userId);
    }
}
