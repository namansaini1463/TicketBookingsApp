using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;
        private readonly IShoppingCartRepository shoppingCartRepository;

        public CartController(TicketBookingsAppDBContext ticketBookingsAppDBContext, IShoppingCartRepository shoppingCartRepository)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
            this.shoppingCartRepository = shoppingCartRepository;
        }

        // POST: api/ShoppingCart/AddItem
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartItemDTO)
        {
            await shoppingCartRepository.AddToCart(cartItemDTO);
            return Ok(new { Message = "Item added to cart successfully!" });
        }

        // GET: api/ShoppingCart/{userId}
        // Get the shopping cart for a specific user
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart([FromRoute]string userId)
        {
            var cart = await shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return NotFound(new { Message = "Shopping cart not found." });
            }

            return Ok(cart);
        }

        // PUT: api/ShoppingCart/UpdateItem
        // Update an existing item in the shopping cart
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDTO updateCartItemDTO)
        {
            var cartItem = await ticketBookingsAppDBContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartItemID == updateCartItemDTO.CartItemID);

            if (cartItem == null)
            {
                return NotFound(new { Message = "Cart item not found." });
            }

            // Update the quantity of the cart item
            cartItem.Quantity = updateCartItemDTO.Quantity;
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return Ok(new { Message = "Cart item updated successfully!" });
        }

        // DELETE: api/ShoppingCart/RemoveItem/{cartItemId}
        // Remove an item from the cart
        [HttpDelete("RemoveItem/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(Guid cartItemId)
        {
            var cartItem = await ticketBookingsAppDBContext.CartItems.FindAsync(cartItemId);

            if (cartItem == null)
            {
                return NotFound(new { Message = "Cart item not found." });
            }

            ticketBookingsAppDBContext.CartItems.Remove(cartItem);
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return Ok(new { Message = "Item removed from cart successfully!" });
        }


    }
}
