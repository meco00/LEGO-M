namespace LegoM.Services.ShoppingCarts.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CartItemServiceModel
    {
        public byte Quantity { get; set; }

        public byte ProductQuantity { get; set; }
    }
}
