namespace LegoM.Services.Orders.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderQueryModel
    {
        public int CurrentPage { get; init; }

        public int OrdersPerPage { get; init; }

        public int TotalOrders { get; init; }

        public IEnumerable<OrderServiceModel> Orders { get; init; }
    }
}
