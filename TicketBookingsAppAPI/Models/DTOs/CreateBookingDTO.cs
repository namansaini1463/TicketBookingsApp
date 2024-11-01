using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class CreateBookingDTO
    {
        public string UserID { get; set; }
        public string CouponCode { get; set; }

        public string paymentMethod { get; set; } 
        public Guid? transactionId { get; set; }
        public PaymentStatus paymentStatus { get; set; }
    }
}
