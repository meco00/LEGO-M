namespace LegoM.Infrastructure
{
  using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Data.Models.Enums;
    using LegoM.Models.Products;
    using LegoM.Services.Products.Models;
    using System;

   

    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
            this.CreateMap<ProductDetailsServiceModel, ProductFormModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => Enum.Parse<ProductCondition>(x.Condition)))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => Enum.Parse<DeliveryTake>(x.Delivery)));

            this.CreateMap<Product, ProductServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x=>x.ProductCondition.ToString()));

            this.CreateMap<Product, ProductDetailsServiceModel>()
            .ForMember(p => p.Condition, pd => pd.MapFrom(x => x.ProductCondition.ToString()))
            .ForMember(p => p.Delivery, pd => pd.MapFrom(x => x.DeliveryTake.ToString()))
            .ForMember(p => p.UserId, pd => pd.MapFrom(x => x.Merchant.UserId));

        }
    }
}
