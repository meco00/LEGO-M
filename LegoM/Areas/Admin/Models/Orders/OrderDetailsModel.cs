namespace LegoM.Areas.Admin.Models.Orders
{
    using LegoM.Services.Orders.Models;
    using LegoM.Services.ShoppingCarts.Models;
    using System.Collections.Generic;

    public class OrderDetailsModel
    {
        public OrderDetailsServiceModel Order { get; set; }

        public IEnumerable<ShoppingCartItemServiceModel> OrderItems { get; set; }

    }
}
