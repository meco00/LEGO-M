namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.ShoppingCarts.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static ProfileConstants;

    public class ShoppingCartItemProfile:Profile
    {
        public ShoppingCartItemProfile()
        {
            this.CreateMap<ShoppingCartItem, ShoppingCartItemServiceModel>()
               .ForMember(x => x.ProductCondition, cfg => cfg.MapFrom(x => x.Product.ProductCondition.ToString()))
               .ForMember(x => x.ProductDelivery, cfg => cfg.MapFrom(x => x.Product.DeliveryTake.ToString()))
               .ForMember(x => x.Quantity, cfg => cfg.MapFrom(x => x.Quantity))
               .ForMember(x => x.ProductQuantity, cfg => cfg.MapFrom(x => x.Product.Quantity))
               .ForMember(x => x.ImageUrl, cfg => cfg.MapFrom(x => x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()))
               .ForMember(x => x.Price, cfg => cfg.MapFrom(x => (x.Product.Price * (decimal)x.Quantity)));

        }
    }
}
