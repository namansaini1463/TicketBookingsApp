using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class CartItem
    {
        [Key]
        public Guid CartItemID { get; set; }

        [Required]
        public Guid CartID { get; set; } // Foreign key for ShoppingCart

        [ForeignKey("CartID")]
        public ShoppingCart ShoppingCart { get; set; }

        [Required]
        public Guid TicketTypeID { get; set; } // Foreign key for TicketType

        [ForeignKey("TicketTypeID")]
        public TicketType TicketType { get; set; }

        [Required]
        public int Quantity { get; set; } // Number of tickets added

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
