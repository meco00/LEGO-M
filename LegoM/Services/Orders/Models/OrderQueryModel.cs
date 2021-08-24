namespace LegoM.Services.Orders.Models
{
    using System.Collections.Generic;

    public class OrderQueryModel
    {
        public int CurrentPage { get; init; }

        public int OrdersPerPage { get; init; }

        public int TotalOrders { get; init; }

        public IEnumerable<OrderServiceModel> Orders { get; init; }
    }
}
