namespace LegoM.Services.Favourites
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.Favourites.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FavouriteService : IFavouriteService
    {
        private readonly LegoMDbContext data;

        private readonly IConfigurationProvider mapper;

        public FavouriteService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Add(string productId, string userId)
        {
            var favourite = new Favourite
            {
                ProductId = productId,
                UserId = userId
            };

            this.data.Favourites.Add(favourite);

            this.data.SaveChanges();

        }

        public IEnumerable<FavouriteServiceModel> All(string userId)
        => this.data.Favourites.Where(x => x.UserId == userId)
            .ProjectTo<FavouriteServiceModel>(mapper);

        public bool Delete(int id)
        {
            var favourite = this.data.Favourites.Find(id);

            if (favourite==null)
            {
                return false;
            }

            this.data.Favourites.Remove(favourite);

            this.data.SaveChanges();

            return true;
            
        }

        public bool IsFavouriteByUser(int id, string userId)
         => this.data.Favourites.Any(x => x.Id == id && x.UserId == userId);

        public bool IsFavouriteExists(string productId, string userId)
        => this.data.Favourites.Any(x => x.ProductId == productId && x.UserId == userId);


    }
}
