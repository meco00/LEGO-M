﻿using LegoM.Areas.Admin.Models.Orders;
using LegoM.Services.Orders;
using LegoM.Services.ShoppingCarts;
using Microsoft.AspNetCore.Mvc;

namespace LegoM.Areas.Admin.Controllers
{
    public class OrdersController:AdminController
    {
        private readonly IOrderService orders;
        private readonly IShoppingCartService shoppingCarts;

        public OrdersController(IOrderService orders, IShoppingCartService shoppingCarts)
        {
            this.orders = orders;
            this.shoppingCarts = shoppingCarts;
        }

        public IActionResult UnAccomplished()
        {
           
            var orders = this.orders.GetOrders();

            return this.View(orders);
        }

        public IActionResult Accomplished()
        {
            var orders = this.orders.GetOrders(IsAccomplished: true);

            return this.View(orders);
        }

        public IActionResult Details(int id)
        {
            ;
            var order = this.orders.Details(id);

            var orderItems=this.shoppingCarts.GetCartItemsbyOrder(id);

            return this.View(new OrderDetailsModel 
            { 
                Order=order,
                OrderItems=orderItems
            });
        }

        public IActionResult Delete(int id)
        {
            
            var IsDeleted = this.orders.Delete(id);

            if (!IsDeleted)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Order was deleted succesfully!";

            return RedirectToAction(nameof(OrdersController.UnAccomplished));
        }

        public IActionResult Accomplish(int id)
        {
            ;
            var IsAccomplished = this.orders.Accomplish(id);

            if (!IsAccomplished)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Order was accomplished succesfully!";

            return RedirectToAction(nameof(UnAccomplished));
        }


        public IActionResult Cancel(int id)
        {
            ;
            var IsCanceled = this.orders.Cancel(id);

            if (!IsCanceled)
            {
                return NotFound();
            }

            this.TempData[WebConstants.GlobalMessageKey] = "Order was canceled and deleted succesfully!";

            return RedirectToAction(nameof(Accomplished));
        }




    }
}
