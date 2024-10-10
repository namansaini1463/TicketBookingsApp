namespace TicketBookingsAppAPI.Models.DTOs
{
    public class EventDTO
    {
        public Guid EventID { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Venue { get; set; }
        public string State { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableTickets { get; set; }
        public string? EventDescription { get; set; }
    }
}
