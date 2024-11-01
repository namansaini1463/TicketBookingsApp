using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class PaymentDTO
    {
        public string paymentMethod = "Credit Card";
        public Guid? transactionId = null;
        public PaymentStatus paymentStatus = PaymentStatus.Pending;
    }
}
