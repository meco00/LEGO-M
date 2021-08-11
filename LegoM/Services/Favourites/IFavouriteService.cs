using LegoM.Services.Favourites.Models;
using System.Collections.Generic;

namespace LegoM.Services.Favourites
{
    public interface IFavouriteService
    {
        void Add(string productId, string userId);

        bool Delete(int id);

        bool IsFavouriteExists(string productId, string userId);

        bool IsFavouriteByUser(int id, string userId);

        IEnumerable<FavouriteServiceModel> All(string userId);
    }
}
