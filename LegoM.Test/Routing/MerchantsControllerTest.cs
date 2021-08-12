namespace LegoM.Test.Routing
{
    using LegoM.Controllers;
    using LegoM.Models.Merchants;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class MerchantsControllerTest
    {
        [Fact]
        public void GetBecomeRouteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Merchants/Become")
                .To<MerchantsController>(c => c.Become());

        [Fact]
        public void PostBecomeRouteShouldBeMapped()
         => MyRouting
             .Configuration()
             .ShouldMap(request=> request
               .WithLocation("/Merchants/Become")
               .WithMethod(HttpMethod.Post))
             .To<MerchantsController>(c => c.Become(With.Any<BecomeMerchantFormModel>()));
    }
}
