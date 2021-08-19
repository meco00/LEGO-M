using LegoM.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace LegoM.Areas.Admin.Controllers
{
    public class OrdersController:AdminController
    {
        private readonly IOrderService orders;

        public OrdersController(IOrderService orders)
        {
            this.orders = orders;
        }

        public IActionResult UnAccomplished()
        {

            var orders = this.orders.GetOrders();

            return this.View(orders);
        }
    }
}
