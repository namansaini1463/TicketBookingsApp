namespace TicketBookingsAppAPI.Models.Domain
{
    public class Booking
    {
        public Guid BookingID { get; set; }
        public string UserID { get; set; }
        public Guid EventID { get; set; }
        public int NumberOfTickets { get; set; } 
        public decimal Amount { get; set; }
        public DateTime BookingDate { get; set; }

        // Navigation Properties
        public User BookingUser { get; set; }
        public Event BookingEvent { get; set; }
    }
}
