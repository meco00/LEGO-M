namespace LegoM.Test.Services
{
    using LegoM.Data.Models;
    using LegoM.Services.Merchants;
    using LegoM.Test.Mocks;
    using Xunit;

    public class MerchantServiceTest
    {
        private const string UserId = "TestUserId";

        [Fact]
        public void IsMerchantrServiceReturnsTrueWhenUserIsMerchant()
        {


            //Arange

            var merchantService = GetMerchantService();
           

            //Act
            var result = merchantService.IsMerchant(UserId);


            //Assert
            Assert.True(result);



        }

        [Fact]
        public void IsMerchantServiceReturnsFalseWhenUserIsNotMerchant()
        {
            //Arange
            var merchantService = GetMerchantService();

            //Act
            var result = merchantService.IsMerchant("AnotherUserId");


            //Assert
            Assert.False(result);

        }

        private IMerchantService GetMerchantService()
        {
          

            var data = DatabaseMock.Instance;
            
            data.Merchants.Add(new Merchant() { UserId = UserId });
            data.SaveChanges();

            return new MerchantService(data);
        }
    }
}
