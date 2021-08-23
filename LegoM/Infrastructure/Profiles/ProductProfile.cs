namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Areas.Admin.Models.Products;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using LegoM.Services.Products.Models;
    using System;
    using System.Linq;

    using static ProfileConstants;

    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            this.CreateMap<ProductDetailsServiceModel, ProductFormModel>()
          .ForMember(p => p.Condition, pd => pd.MapFrom(x => Enum.Parse<ProductCondition>(x.Condition)))
          .ForMember(p => p.Delivery, pd => pd.MapFrom(x => Enum.Parse<DeliveryTake>(x.Delivery)))
          .ForMember(p => p.FirstImageUrl, pd => pd.MapFrom(x => x.MainImageUrl));

            this.CreateMap<ProductDetailsServiceModel, ProductModel>()
            .ForMember(p => p.Image, pd => pd.MapFrom(x => x.MainImageUrl));

            this.CreateMap<Product, ProductServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
            .ForMember(x => x.DeletedOn, cfg => cfg.MapFrom(x => x.DeletedOn.HasValue ? x.DeletedOn.Value.ToString(DateTimeFormat):string.Empty))

            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Product, ProductDetailsServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => x.DeliveryTake.ToString()))
            .ForMember(p => p.UserId, pd => pd.MapFrom(x => x.Trader.UserId))
            .ForMember(p => p.MainImageUrl, pd => pd.MapFrom(x => x.Images.Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.SecondImageUrl, pd => pd.MapFrom(x => x.Images.Skip(1).Take(1).Select(x => x.ImageUrl).FirstOrDefault()))
            .ForMember(p => p.ThirdImageUrl, pd => pd.MapFrom(x => x.Images.Skip(2).Take(1).Select(x => x.ImageUrl).FirstOrDefault()));

        }

    }
}
