namespace LegoM.Services.Merchants
{
    using LegoM.Data;
    using System.Linq;

    public class MerchantService : IMerchantService
    {
        private readonly LegoMDbContext data;

        public MerchantService(LegoMDbContext data)
        {
            this.data = data;
        }

        

        public string IdByUser(string userId)
        => this.data.Merchants
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();

        public bool IsMerchant(string userId)
        => this.data.Merchants.Any(x=>x.UserId==userId);
    }
}
