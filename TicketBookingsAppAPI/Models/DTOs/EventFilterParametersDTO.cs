using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class EventFilterParametersDTO
    {
        public string? searchTerm { get; set; }
        public string? sortBy { get; set; }
        public string? sortOrder { get; set; }
        public DateTime? StartDate { get; set; } // Start of the date range
        public DateTime? EndDate { get; set; }   // End of the date range

        public EventCategory? Category { get; set; } // Category for filtering
    }
}
