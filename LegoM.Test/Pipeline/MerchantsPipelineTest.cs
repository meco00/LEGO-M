namespace LegoM.Test.Pipeline
{
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Products;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static WebConstants;

    public class MerchantsPipelineTest
    {

        [Fact]
        public void BecomeShouldBeForAuthorizedUsersAndReturnView()
            =>MyPipeline
            .Configuration()
            .ShouldMap(request=> request.WithLocation("/Merchants/Become")
            .WithUser())
            .To<MerchantsController>(c=>c.Become())      
            .Which()
            .ShouldHave()           
            .ActionAttributes(attributes=>attributes
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
          =>MyPipeline
            .Configuration()
             .ShouldMap(request=>request
              .WithLocation("/Merchants/Become")
              .WithMethod(HttpMethod.Post)
               .WithFormFields(new
               {
                 Name =  merchantName,
                 TelephoneNumber = phoneNumber
               })
              .WithUser()
                  .WithAntiForgeryToken())
                .To<MerchantsController>(c=>c.Become(new Models.Merchants.BecomeMerchantFormModel
                {
                    Name = merchantName,
                    TelephoneNumber = phoneNumber
                } ))                   
              .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Merchant>(set => set
                             .Any(x =>
                             x.Name == merchantName &&
                             x.TelephoneNumber == phoneNumber &&
                             x.UserId == TestUser.Identifier)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                      .To<ProductsController>(c => c
                      .All(With.Any<ProductsQueryModel>())));



    }
}
