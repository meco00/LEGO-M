namespace LegoM.Test.Controllers
{
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Products;
    using MyTested.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    using static WebConstants;

    public class MerchantsControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndShouldReturnView()
            => MyController<TradersController>
                .Instance()                     
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .View();

        [Theory]
        [InlineData("Trader", "0885412589")]
        public void PostBecomeShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
            string merchantName,
            string phoneNumber
            )
            => MyController<TradersController>
                .Instance(controller=>controller
                          .WithUser())
                 .Calling(c => c.Become(new Models.Traders.BecomeTraderFormModel
                 {
                     Name=merchantName,
                     TelephoneNumber=phoneNumber

                 }))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                     .RestrictingForAuthorizedRequests()
                      .RestrictingForHttpMethod(HttpMethod.Post))
                  .ValidModelState()
                  .Data(data=>data
                       .WithSet<Trader>(merchants=>merchants
                               .Any(x => 
                               x.Name == merchantName && 
                               x.TelephoneNumber == phoneNumber &&
                               x.UserId==TestUser.Identifier)))
                   .TempData(tempData=>tempData
                            .ContainingEntryWithKey(GlobalMessageKey))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect=>redirect
                        .To<ProductsController>(c=>c
                        .All(With.Any<ProductsQueryModel>())));

            
            
            

    }
}
