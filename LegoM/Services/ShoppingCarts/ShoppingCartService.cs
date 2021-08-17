﻿namespace LegoM.Services.ShoppingCarts
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.ShoppingCarts.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly LegoMDbContext data;

        private readonly IConfigurationProvider mapper;

        public ShoppingCartService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }


        public void Add(string productId, string userId)
        {
            var shoppingCartItem = new ShoppingCartItem
            {
                ProductId = productId,
                UserId = userId,
                Quantity=1
            };

            this.data.ShoppingCarts.Add(shoppingCartItem);

            this.data.SaveChanges();

        }

        public bool Delete(int id)
        {
            var item = this.data.ShoppingCarts.Find(id);

            if (item==null)
            {
                return false;
            }

            this.data.ShoppingCarts.Remove(item);

            this.data.SaveChanges();

            return true;
        }

        public ShoppingCartItemServiceModel Details(int id)
        => this.data.ShoppingCarts.Where(x => x.Id == id)
            .ProjectTo<ShoppingCartItemServiceModel>(mapper)
            .FirstOrDefault();

        public bool Edit(int id, byte Quantity)
        {
            var shoppingCartItem = this.data.ShoppingCarts.Include(x=>x.Product).FirstOrDefault(x=>x.Id==id);

            if (shoppingCartItem == null)
            {
                return false;
            }

            if (shoppingCartItem.Product.Quantity < Quantity)
            {
                return false;
            }

            shoppingCartItem.Quantity = Quantity;

            this.data.SaveChanges();

            return true;

        }

        public CartItemServiceModel GetQuantityAndProductQuantity(int id)
        {
            var cartItem = this.Details(id);

            if (cartItem==null)
            {
                return null;
            }

            return new CartItemServiceModel
            {
                Quantity = cartItem.Quantity,
                ProductQuantity = cartItem.ProductQuantity
            };
        }

        public bool ItemExists(string productId, string userId)
        => this.data.ShoppingCarts.Any(x => x.ProductId == productId && x.UserId == userId);

        public bool ItemIsByUser(int id, string userId)
        => this.data.ShoppingCarts.Any(x => x.Id == id && x.UserId == userId);

        public IEnumerable<ShoppingCartItemServiceModel> Mine(string userId)
          => this.data.ShoppingCarts.Where(x => x.UserId == userId)
            .ProjectTo<ShoppingCartItemServiceModel>(mapper)
            .ToList();
    }
}
