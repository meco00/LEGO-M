using LegoM.Services.ShoppingCarts.Models;
using System.Collections.Generic;

namespace LegoM.Services.ShoppingCarts
{
    public interface IShoppingCartService
    {
        void Add(
            string productId,
            string userId);

        bool Delete(int id);

        bool Edit(
            int id,
            byte Quantity);

        bool ItemIsByUser(int id, string userId);

        bool ItemExists(string productId, string userId);

        ShoppingCartItemServiceModel Details(int id);

        CartItemServiceModel  GetQuantityAndProductQuantity(int id);

        IEnumerable<ShoppingCartItemServiceModel> Mine(string userId);

        bool UserHasAnyUnOrderedShoppingCartItem(string userId);

        IEnumerable<string> GetInformationAboutInvalidShoppingCartItemsOfUser(string userId);

        IEnumerable<string> ValidateShoppingCartOfUser(string userId);

        IEnumerable<ShoppingCartItemServiceModel> GetCartItemsbyOrder(int orderId);
    }
}
