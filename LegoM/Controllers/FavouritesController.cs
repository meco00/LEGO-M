﻿namespace LegoM.Controllers
{
    using LegoM.Infrastructure;
    using LegoM.Services.Favourites;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FavouritesController:Controller
    {
        private readonly IFavouriteService favourites;

        public FavouritesController(IFavouriteService favourites)
        {
            this.favourites = favourites;
        }

        [Authorize]
        public IActionResult Add(string id)
        {
            var userId = this.User.Id();

            if (this.User.IsAdmin() || this.favourites.IsFavouriteExists(id,userId))
            {
                return BadRequest();
            }

            this.favourites.Add(id, userId);


            this.TempData[WebConstants.GlobalMessageKey] = "Product succesfully was added to favourites!";

            return RedirectToAction(nameof(ProductsController.Details), "Products", new { id });

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();

            if (this.User.IsAdmin() || !this.favourites.IsFavouriteByUser(id, userId))
            {
                return BadRequest();
            }

            var IsDeleted = this.favourites.Delete(id);

            if (!IsDeleted)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(All), new { id = userId });


        }

        [Authorize]
        public IActionResult All()
        {
            var favourites = this.favourites.All(this.User.Id());


            return View(favourites);
        }

    }
}