namespace LegoM.Test.Controllers.Admin
{
    using FluentAssertions;
    using LegoM.Services.Products.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    using static Data.Products;

    using static LegoM.Areas.Admin.AdminConstants;

    using AdminController = LegoM.Areas.Admin.Controllers.AdminController;

    using ProductsController = LegoM.Areas.Admin.Controllers.ProductsController;



    public class ProductControllerTest
    {
        [Fact]
        public void ControllerShouldBeInAdminArea()
            => MyController<AdminController>
                .ShouldHave()
                 .Attributes(attributes => attributes
                     .SpecifyingArea(AreaName)
                     .RestrictingForAuthorizedRequests(AdministratorRoleName));
                
                

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<ProductsController>                              
                .Instance(controller => controller
                .WithData(TenPublicProducts()))
                .Calling(x => x.All())            
                 .ShouldReturn()
                 .View(view => view.WithModelOfType<List<ProductServiceModel>>()
                 .Passing(model => model.Should().HaveCount(10)));

        [Fact]
        public void DeletedShouldReturnCorrectViewWithModel()
            => MyController<ProductsController>
               .Instance(controller => controller
                .WithData(TenDeletedProducts()))
                .Calling(x => x.Deleted())
                .ShouldReturn()
                .View(view => view.WithModelOfType<List<ProductDeletedServiceModel>>()
                .Passing(model => model.Should().HaveCount(10)));
            
                
               
    }
}
