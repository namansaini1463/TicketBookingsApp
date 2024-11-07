using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class Booking
    {
        [Key]
        public Guid BookingID { get; set; }

        [Required]
        public string UserID { get; set; } // Link to the user who made the booking
        [ForeignKey("UserID")]
        public User User { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public BookingStatus BookingStatus { get; set; }

        // Collection of BookingItems representing each ticket booked
        public List<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

        [Required]
        public decimal TotalAmount { get; set; } // Total price for the booking

        // Coupon reference
        public Guid? CouponID { get; set; } // Nullable foreign key for Coupon
        [ForeignKey("CouponID")]
        public Coupon? Coupon { get; set; }

        public decimal DiscountApplied { get; set; } // Discount amount applied
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
