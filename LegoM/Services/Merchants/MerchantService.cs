namespace LegoM.Services.Merchants
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using System.Linq;

    public class MerchantService : IMerchantService
    {
        private readonly LegoMDbContext data;

        public MerchantService(LegoMDbContext data)
        {
            this.data = data;
        }

        public void Create(string userId, string name, string telephoneNumber)
        {
            var merchant = new Merchant
            {
                Name = name,
                TelephoneNumber =telephoneNumber,
                UserId = userId
            };

            data.Merchants.Add(merchant);

            data.SaveChanges();
        }

        public string IdByUser(string userId)
        => this.data.Merchants
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();

        public bool IsUserMerchant(string userId)
        => this.data.Merchants.Any(x=>x.UserId==userId);
    }
}
