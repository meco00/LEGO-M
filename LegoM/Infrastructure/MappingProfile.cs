namespace LegoM.Infrastructure
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using LegoM.Models.Reviews;
    using LegoM.Services.Products.Models;
    using LegoM.Services.Reviews.Models;
    using System;
    using System.Linq;

    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
            this.CreateMap<ProductDetailsServiceModel, ProductFormModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => Enum.Parse<ProductCondition>(x.Condition)))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => Enum.Parse<DeliveryTake>(x.Delivery)))
            .ForMember(p => p.FirstImageUrl, pd => pd.MapFrom(x => x.MainImageUrl)) 
            .ForMember(p => p.SecondImageUrl, pd => pd.MapFrom(x => x.SecondImageUrl)) 
            .ForMember(p => p.ThirdImageUr, pd => pd.MapFrom(x => x.ThirdImageUrl)) ;

            this.CreateMap<Product, ProductServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Where(x => x.isDeleted == false).Select(x => x.ImageUrl).FirstOrDefault()));        

            this.CreateMap<Product, ProductDetailsServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => x.DeliveryTake.ToString()))
            .ForMember(p => p.UserId, pd => pd.MapFrom(x => x.Merchant.UserId))
            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.SecondImageUrl, pd => pd.MapFrom(x => x.Images.Skip(1).Take(1).Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.ThirdImageUrl, pd => pd.MapFrom(x => x.Images.Skip(2).Take(1).Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Category, ProductCategoryServiceModel>();
            this.CreateMap<SubCategory, ProductSubCategoryServiceModel>();

            this.CreateMap<Review, ReviewByUserServiceModel>()
                .ForMember(x=>x.Information,cfg=>cfg.MapFrom(x =>String.Concat(x.Title + "-" + x.Rating + "-" + x.PublishedOn.ToString("MM MMM yyy"))));

            this.CreateMap<Review, ReviewListingServiceModel>()
                 .ForMember(x => x.Rating, cfg => cfg.MapFrom(x => (int)x.Rating))
                 .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x=>x.PublishedOn.ToString("MM MMM yyy")))
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

