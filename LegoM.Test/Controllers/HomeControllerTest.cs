
namespace LegoM.Test.Controllers
{
    using AutoMapper;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.Products;
    using LegoM.Services.Statistics;
    using LegoM.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using Xunit;
    using FluentAssertions;
    using MyTested.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using LegoM.Services.Products.Models;
    using System.Collections.Generic;

    public class HomeControllerTest
    {

        [Fact]
        public void IndexShouldReturnViewWithCorrectDataAndModel()
       =>   //Arrange

            MyMvc
            .Pipeline()
            .ShouldMap("/")

            //Act
            .To<HomeController>(c=>c.Index())
            .Which(controller => controller
            .WithData(Enumerable.Range(0, 10).Select(p => new Product())))           
             

            //Assert
            .ShouldReturn()
            .View(view=>view.WithModelOfType<List<ProductServiceModel>>()
            .Passing(m=>m.Should().HaveCount(3)));

        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            //Arange
            using var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;
            var memoryCache = MemoryCacheMock.Instance;

            data.Products.AddRange(
                Enumerable.Range(0, 10)
                .Select(i => new Product()));

            data.Users.AddRange(Enumerable.Range(0, 2)
                .Select(i => new User()));

            data.SaveChanges();

            var productService = new ProductsService(data,mapper);
            


            var homeController = new HomeController(productService,memoryCache);

            //Act
            var result = homeController.Index();

            //Assert
            result.Should().NotBeNull()
                .And
                .BeAssignableTo<ViewResult>()
                .Which
                .Model
                .As<List<ProductServiceModel>>()
                .Invoking(model =>
                {
                    model.Should().HaveCount(3);
                   

                })
                .Invoke();



          //  Assert.NotNull(result);

          //var viewResult=Assert.IsType<ViewResult>(result);

          //  var model = viewResult.Model;

          //var indexViewModel=Assert.IsType<IndexViewModel>(model);

          //  Assert.Equal(3, indexViewModel.Products.Count);
          //  Assert.Equal(10, indexViewModel.TotalProducts);
          //  Assert.Equal(2, indexViewModel.TotalUsers);
           

        }


        [Fact]
        public void ErrorShouldReturnView()
        {
            //Arrange
            var homeController = new HomeController(null,null);
            //Act
            var result=homeController.Error();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
