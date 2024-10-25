using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public interface IShoppingCartRepository
    {
        Task AddToCart(CartItemDTO cartItemDTO);
        //Task<CartItem> GetCartItems();
        Task<ShoppingCart> GetCartByUserIdAsync(string userId);
    }
}
