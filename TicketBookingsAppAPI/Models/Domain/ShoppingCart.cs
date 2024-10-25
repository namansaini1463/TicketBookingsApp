using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class ShoppingCart
    {
        [Key]
        public Guid CartID { get; set; }

        [Required]
        public string UserID { get; set; } // Link to the user who owns the cart

        public User User { get; set; } // Navigation property


        // Collection of CartItems
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
