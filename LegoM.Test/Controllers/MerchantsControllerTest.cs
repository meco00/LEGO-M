namespace LegoM.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MyTested.AspNetCore.Mvc;
   
    using Xunit;
    using System.Threading.Tasks;
    using LegoM.Controllers;
    using LegoM.Data.Models;

    using static WebConstants;
    using LegoM.Models.Products;

    public class MerchantsControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndShouldReturnView()
            => MyController<MerchantsController>
                .Instance()                     
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
                 .AndAlso()
                 .ShouldReturn()
                 .View();

        [Theory]
        [InlineData("Merchant", "0885412589")]
        public void PostBecomeShouldBeForAuthorizedUsersAndShoulReturnRedirectToViewWithCorrectData(
            string merchantName,
            string phoneNumber
            )
            => MyController<MerchantsController>
                .Instance(controller=>controller
                          .WithUser())
                 .Calling(c => c.Become(new Models.Merchants.BecomeMerchantFormModel
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
                       .WithSet<Merchant>(merchants=>merchants
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
