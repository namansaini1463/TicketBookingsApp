using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public class SQLShoppingCartRepository : IShoppingCartRepository
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;

        public SQLShoppingCartRepository(TicketBookingsAppDBContext ticketBookingsAppDBContext)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
        }
        public async Task AddToCart(CartItemDTO cartItemDTO)
        {
            // Retrieve or create the shopping cart for the user
            var cart = await GetCartByUserIdAsync(cartItemDTO.userID);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    CartID = Guid.NewGuid(),
                    UserID = cartItemDTO.userID,
                };

                await ticketBookingsAppDBContext.ShoppingCarts.AddAsync(cart);
                await ticketBookingsAppDBContext.SaveChangesAsync();
            }

            // Check if the ticket type already exists in the cart
            var existingCartItem = cart.CartItems
                .FirstOrDefault(ci => ci.TicketTypeID == cartItemDTO.TicketTypeID);

            if (existingCartItem != null)
            {
                // Update the quantity if it already exists
                existingCartItem.Quantity += cartItemDTO.Quantity;
            }
            else
            {
                // Add a new cart item
                var cartItem = new CartItem
                {
                    CartID = cart.CartID,
                    TicketTypeID = cartItemDTO.TicketTypeID,
                    Quantity = cartItemDTO.Quantity,
                    DateAdded = DateTime.UtcNow
                };

                cart.CartItems.Add(cartItem);
            }

            await ticketBookingsAppDBContext.SaveChangesAsync();
        }


        public async Task<ShoppingCart?> GetCartByUserIdAsync(string userId)
        {
            return await ticketBookingsAppDBContext.ShoppingCarts
                .Include(ci => ci.User)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.TicketType)
                .FirstOrDefaultAsync(c => c.UserID == userId);
        }
    }
}