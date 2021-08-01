
namespace LegoM.Test.Controllers
{
    using AutoMapper;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using LegoM.Models.Home;
    using LegoM.Services.Products;
    using LegoM.Services.Statistics;
    using LegoM.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Linq;
    using Xunit;
   

    public class HomeControllerTest
    {
      
        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            //Arange
            using var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            data.Products.AddRange(
                Enumerable.Range(0, 10)
                .Select(i => new Product()));

            data.Users.AddRange(Enumerable.Range(0, 2)
                .Select(i => new User()));

            data.SaveChanges();

            var productService = new ProductsService(data,mapper);
            var statisticsService = new StatisticsService(data);

            var homeController = new HomeController(statisticsService, productService);

            //Act
            var result = homeController.Index();

            //Assert
            Assert.NotNull(result);

          var viewResult=Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

          var indexViewModel=Assert.IsType<IndexViewModel>(model);

            Assert.Equal(3, indexViewModel.Products.Count);
            Assert.Equal(10, indexViewModel.TotalProducts);
            Assert.Equal(2, indexViewModel.TotalUsers);
           

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
