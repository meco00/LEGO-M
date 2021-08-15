namespace LegoM.Test.Controllers
{
    using LegoM.Controllers;
    using LegoM.Models.Products;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Users;
    using static Data.Categories;
    using LegoM.Data.Models.Enums;
    using LegoM.Data.Models;

    public class ProductsControllerTest
    {
        [Fact]
        public void PosstAddShouldBeForAuthorizedUsersAndReturnRedirectToAndCorrectData()
            => MyController<ProductsController>
                .Instance(controller => controller
                        .WithUser()
                        .WithData(GetMerchant())
                         .WithData(GetCategories()))
                 .Calling(c => c.Add(new ProductFormModel
                 {
                     Title = "TitleTest",
                     Description = "DescriptionTest",
                     Price = 12.50M,
                     Quantity = 2,
                     FirstImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/44/Cat_img.jpg",
                     CategoryId = 1,
                     SubCategoryId = 1,
                     Condition = ProductCondition.New,
                     Delivery = DeliveryTake.Seller,
                     AgreeOnTermsOfPolitics=true

                 }))
                .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                     .WithSet<Product>(set => set
                             .Any(x =>
                             x.Title == "TitleTest" &&
                             x.Description == "DescriptionTest" &&
                             x.Price == 12.50M &&
                             x.Quantity == 2 &&
                             x.Images.Any() &&
                             x.CategoryId == 1 &&
                             x.SubCategoryId == 1 &&
                             x.IsPublic==false &&
                             x.ProductCondition == ProductCondition.New &&
                             x.DeliveryTake == DeliveryTake.Seller)))
                 .TempData(tempData => tempData
                          .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                       .To<ProductsController>(c => c
                       .Details(With.Any<string>())));
                 




    }
}
