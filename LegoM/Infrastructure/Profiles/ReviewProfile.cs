namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Reviews;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Linq;

    public class ReviewProfile:Profile
    {
        public ReviewProfile()
        {
            this.CreateMap<Review, ReviewByUserServiceModel>()
             .ForMember(x => x.Information, cfg => cfg.MapFrom(x => String.Concat(x.Title + "-" + x.Rating + "-" + x.PublishedOn.ToString("MM MMM yyy"))));

            this.CreateMap<Review, ReviewListingServiceModel>()
                 .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                 .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString("MM MMM yyy")))
                 .ForMember(x => x.ProductImage, cfg => cfg.MapFrom(x => x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault())
                 );

            this.CreateMap<Review, ReviewServiceModel>()
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString("MM MMM yyy")));


            this.CreateMap<Review, ReviewDetailsServiceModel>()
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                .ForMember(x => x.ProductPrice, cfg => cfg.MapFrom(x => x.Product.Price.ToString("f2")))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString("MM MMM yyy")))
                .ForMember(x => x.ProductImage, cfg => cfg.MapFrom(x => x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<ReviewDetailsServiceModel, ReviewFormModel>()
                .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (ReviewType)x.Rating));

        }
    }
}
