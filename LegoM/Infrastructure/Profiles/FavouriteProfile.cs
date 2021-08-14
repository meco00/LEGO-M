namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Favourites.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static ProfileConstants;

    public class FavouriteProfile:Profile
    {
        public FavouriteProfile()
        {
            this.CreateMap<Favourite, FavouriteServiceModel>()
                .ForMember(x => x.Price, cfg => cfg.MapFrom(x => x.Product.Price.ToString(PriceFormat)))
                .ForMember(x => x.ProductCondition, cfg => cfg.MapFrom(x => x.Product.ProductCondition.ToString()))
                .ForMember(x => x.ProductDelivery, cfg => cfg.MapFrom(x => x.Product.DeliveryTake.ToString()))
                .ForMember(x => x.Quantity, cfg => cfg.MapFrom(x => x.Product.Quantity))
                .ForMember(x => x.ReviewsCount, cfg => cfg.MapFrom(x => x.Product.Reviews.Count))
                .ForMember(x => x.QuestionsCount, cfg => cfg.MapFrom(x => x.Product.Questions.Count))
                .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()));

          

        }
    }
}
