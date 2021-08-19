namespace LegoM.Services.Orders
{
  using LegoM.Services.Orders.Models;
    using System.Collections.Generic;

    public interface IOrderService
    {
        void Add(
            string fullName,
            string telephoneNumber,
            string state,
            string city,
            string address,
            string zipCode,
            string userId);

        OrderFormServiceModel GetOrderAddFormModel(string userId);

        OrderDetailsServiceModel Details(int id);

        IEnumerable<OrderServiceModel> GetOrders(bool IsAccomplished = false);

        bool Delete(int id);

        bool Cancel(int id);

        bool Accomplish(int id);
    }
}
