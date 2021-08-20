namespace LegoM.Test.Pipeline
{
    using FluentAssertions;
    using LegoM.Controllers;
    using LegoM.Data.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.DataConstants;
    using static Data.Products;
    using static Data.Favourites;
    using LegoM.Services.Favourites.Models;
    using LegoM.Areas.Admin;
    using LegoM.Models.Products;

    public class FavouritesPipelineTest
    {
        [Fact]
        public void AddShouldReturnCorrectDataAndRedirectTo()
            => MyPipeline
                    .Configuration()
                    .ShouldMap(request => request
                       .WithPath($"/Favourites/Add/{TestId}")
                       .WithUser())
                     .To<FavouritesController>(c => c.Add(TestId))
                      .Which(controller => controller.WithData(GetProduct()))
                      .ShouldHave()
                      .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests())
                       .Data(data => data.WithSet<Favourite>(set =>
                       {
                           var favourite = set.FirstOrDefault();

                           favourite.ProductId.Should().Be(TestId);

                           favourite.UserId.Should().Be(TestUser.Identifier);

                       }))
                       .TempData(tempData => tempData
                       .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                       .AndAlso()
                       .ShouldReturn()
                       .Redirect(redirect => redirect
                             .To<ProductsController>(c => c
                             .Details(TestId,With.Any<ProductsDetailsQueryModel>())));

        [Fact]
        public void AddShouldReturnNotFoundWhenProductDoesNotExists()
      => MyPipeline
              .Configuration()
              .ShouldMap(request => request
                 .WithPath($"/Favourites/Add/{TestId}")
                 .WithUser())
               .To<FavouritesController>(c => c.Add(TestId))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                      .RestrictingForAuthorizedRequests())
                  .AndAlso()
                 .ShouldReturn()
                 .NotFound();

        [Fact]
        public void AddShouldReturnBadRequestWhenUserIsAdmin()
          => MyPipeline
                  .Configuration()
                  .ShouldMap(request => request
                     .WithPath($"/Favourites/Add/{TestId}")
                     .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                   .To<FavouritesController>(c => c.Add(TestId))
                    .Which(controller=>controller.WithData(GetProduct()))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                      .AndAlso()
                     .ShouldReturn()
                     .BadRequest();
                    

        [Fact]
        public void MineShouldReturnViewAndCorrectDataAndModel()
            => MyPipeline
                   .Configuration()
                   .ShouldMap(request => request
                       .WithPath("/Favourites/Mine")
                        .WithUser())
                    .To<FavouritesController>(c => c.Mine())
                    .Which(controller => controller.WithData(GetFavourites(5, true, false)))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests())
                     .AndAlso()
                     .ShouldReturn()
                     .View(view => view
                           .WithModelOfType<List<FavouriteServiceModel>>()
                           .Passing(model => model.Should().HaveCount(5)));

        


        [Fact]
        public void DeleteShouldDeleteFavouriteAndReturnRedirectToAllWithCorrectData()
            => MyPipeline
                   .Configuration()
                   .ShouldMap(request => request
                       .WithPath($"/Favourites/Delete/{1}")
                        .WithUser())
                    .To<FavouritesController>(c => c.Delete(1))
                    .Which(controller => controller.WithData(GetFavourites(1)))
                    .ShouldHave()
                    .ActionAttributes(attributes => attributes
                            .RestrictingForAuthorizedRequests())
                      .Data(data => data.WithSet<Favourite>(set =>
                      {
                          set.FirstOrDefault(x => x.Id == 1).Should().BeNull();
                          set.Should().BeEmpty();
                      }))
                     .TempData(tempData => tempData
                            .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                      .AndAlso()
                      .ShouldReturn()
                      .Redirect(redirect => redirect
                              .To<FavouritesController>(c => c
                              .Mine()));


        [Fact]
        public void DeleteShouldReturnBadRequestWhenUserIsAdmin()
         => MyPipeline
                 .Configuration()
                 .ShouldMap(request => request
                    .WithPath($"/Favourites/Delete/{1}")
                    .WithUser(new[] { AdminConstants.AdministratorRoleName }))
                  .To<FavouritesController>(c => c.Delete(1))
                   .Which()
                   .ShouldHave()
                   .ActionAttributes(attributes => attributes
                         .RestrictingForAuthorizedRequests())
                     .AndAlso()
                    .ShouldReturn()
                    .BadRequest();




    }
}
