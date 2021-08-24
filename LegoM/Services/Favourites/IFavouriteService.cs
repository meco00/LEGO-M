namespace LegoM.Services.Favourites
{
    using LegoM.Services.Favourites.Models;
    using System.Collections.Generic;

    public interface IFavouriteService
    {
        void Add(string productId, string userId);

        bool Delete(int id);

        bool IsFavouriteExists(string productId, string userId);

        bool IsFavouriteByUser(int id, string userId);

        IEnumerable<FavouriteServiceModel> Mine(string userId);
    }
}
