namespace LegoM.Services.Orders
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.Merchants;
    using LegoM.Services.Orders.Models;
    using LegoM.Services.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly LegoMDbContext data;
        private readonly IConfigurationProvider mapper;
        private readonly IMerchantService merchants;
        private readonly IUserService users;

        public OrderService(LegoMDbContext data, IMerchantService merchants, IUserService users, IMapper mapper)
        {
            this.data = data;
            this.merchants = merchants;
            this.users = users;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Add(string fullName,
            string telephoneNumber,
            string state,
            string city,
            string address,
            string zipCode,
            string userId)
        {
            var shoppingCart = this.data
                .ShoppingCarts
                .Where(x => x.UserId == userId && x.OrderId == null);


            var order = new Order
            {
                FullName = fullName,
                PhoneNumber = telephoneNumber,
                State = state,
                City = city,
                Address = address,
                ZipCode = zipCode,
                UserId = userId,
                ShoppingCart = shoppingCart.ToList(),
                OrderedOn = DateTime.UtcNow,
                IsAccomplished = false
            };


            this.data.Orders.Add(order);

            this.data.SaveChanges();

        }

        public OrderFormServiceModel GetOrderAddFormModel(string userId)
        {
            var userFullName = this.users.GetFullName(userId);

            var telephoneNumber = this.merchants.TelephoneNumberByUser(userId);

            return new OrderFormServiceModel
            {
                FullName = userFullName,
                TelephoneNumber = telephoneNumber
            };
           
        }

        public IEnumerable<OrderServiceModel> GetOrders(bool IsAccomplished = false)
        => this.data.Orders
            .Where(x => x.IsAccomplished == IsAccomplished)
            .OrderBy(x=>x.OrderedOn)
            .ProjectTo<OrderServiceModel>(mapper)
            .ToList();
    }
}
