using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class BookingDTO
    {
        public Guid BookingID { get; set; }
        public Guid UserID { get; set; }
        public Guid EventID { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal Amount { get; set; }
        public DateTime BookingDate { get; set; }

        public User BookingUser { get; set; }
        public Event BookingEvent { get; set; }
    }
}
