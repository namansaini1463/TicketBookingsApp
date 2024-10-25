using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class BookingItem
    {
        [Key]
        public Guid BookingItemID { get; set; }

        [Required]
        public Guid BookingID { get; set; } // Foreign key for Booking
        public Booking Booking { get; set; }

        [Required]
        public Guid TicketTypeID { get; set; } // Foreign key for TicketType
        public TicketType TicketType { get; set; }

        [Required]
        public int Quantity { get; set; } // Number of tickets booked
    }
}
