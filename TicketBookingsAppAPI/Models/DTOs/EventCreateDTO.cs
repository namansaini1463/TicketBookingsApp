using System.ComponentModel.DataAnnotations;
using TicketBookingsAppAPI.Models.Domain;

public class EventCreateDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public DateTimeOffset DateAndTime { get; set; }

    [Required]
    public VenueDTO Venue { get; set; }  // Venue details

    [MaxLength(2000)]
    public string? EventDescription { get; set; }

    [Required]
    public List<EventCategory> Categories { get; set; } = new List<EventCategory>();

    [Required]
    public List<TicketTypeDTO> TicketTypes { get; set; } = new List<TicketTypeDTO>();  // Ticket types

    public List<EventImageDTO> Images { get; set; } = new List<EventImageDTO>();  // Event images

    [Required]
    public OrganizerDTO Organizer { get; set; }  // Organizer details
}
