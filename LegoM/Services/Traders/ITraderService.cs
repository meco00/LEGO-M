namespace LegoM.Services.Traders
{
   public interface ITraderService
    {
        void Create(
            string userId,
            string name,
            string telephoneNumber
            );

            

        public bool IsUserTrader(string userId);

        string IdByUser(string userId);

        string TelephoneNumberByUser(string userId);
    }
}
