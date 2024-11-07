using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class Payment
    {
        [Key]
        public Guid PaymentID { get; set; }

        [Required]
        public Guid BookingID { get; set; } // Foreign key for Booking

        [ForeignKey("BookingID")]
        public Booking Booking { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; 
        public DateTime? RefundDate { get; set; } = null; 

        public string PaymentMethod { get; set; } // E.g., Credit Card, PayPal, etc.

        public PaymentStatus PaymentStatus { get; set; }

        public Guid TransactionID { get; set; } // Transaction reference from payment provider
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }
}
