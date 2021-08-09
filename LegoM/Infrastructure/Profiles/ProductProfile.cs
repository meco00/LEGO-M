namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using LegoM.Services.Products.Models;
    using System;
    using System.Linq;

    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            this.CreateMap<ProductDetailsServiceModel, ProductFormModel>()
          .ForMember(p => p.Condition, pd => pd.MapFrom(x => Enum.Parse<ProductCondition>(x.Condition)))
          .ForMember(p => p.Delivery, pd => pd.MapFrom(x => Enum.Parse<DeliveryTake>(x.Delivery)))
          .ForMember(p => p.FirstImageUrl, pd => pd.MapFrom(x => x.MainImageUrl))
          .ForMember(p => p.SecondImageUrl, pd => pd.MapFrom(x => x.SecondImageUrl))
          .ForMember(p => p.ThirdImageUr, pd => pd.MapFrom(x => x.ThirdImageUrl));

            this.CreateMap<Product, ProductServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Product, ProductDetailsServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => x.DeliveryTake.ToString()))
            .ForMember(p => p.UserId, pd => pd.MapFrom(x => x.Merchant.UserId))
            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.SecondImageUrl, pd => pd.MapFrom(x => x.Images.Skip(1).Take(1).Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.ThirdImageUrl, pd => pd.MapFrom(x => x.Images.Skip(2).Take(1).Select(x => x.ImageUrl).FirstOrDefault()));

        }

    }
}
