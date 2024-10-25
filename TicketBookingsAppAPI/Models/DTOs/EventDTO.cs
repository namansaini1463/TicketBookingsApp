using TicketBookingsAppAPI.Models.Domain;
public class EventDTO
{
    public Guid EventID { get; set; }

    public string Name { get; set; }

    public DateTimeOffset DateAndTime { get; set; }

    public VenueDTO Venue { get; set; }

    public string? EventDescription { get; set; }

    public List<EventCategory> Categories { get; set; }

    public List<TicketTypeDTO> TicketTypes { get; set; }

    public List<EventImageDTO> Images { get; set; }

    public OrganizerDTO Organizer { get; set; }

    // Calculated field for available tickets
    public int AvailableTickets { get; set; }

    // Total ticket price (calculated from TicketTypes)
    public decimal TotalTicketPrice { get; set; }
}

public class VenueDTO
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string State { get; set; }
    public int Capacity { get; set; }
}
public class TicketTypeDTO
{
    public string Type { get; set; }  // Ticket type (e.g., VIP, General Admission)
    public decimal Price { get; set; }  // Price for this ticket type
    public int QuantityAvailable { get; set; }  // Number of available tickets for this type
    public string TicketTypeID { get; set; } // Ticket ID
}
public class CreateTicketTypeDTO
{
    public string Type { get; set; }  // Ticket type (e.g., VIP, General Admission)
    public decimal Price { get; set; }  // Price for this ticket type
    public int QuantityAvailable { get; set; }  // Number of available tickets for this type
}
public class EventImageDTO
{
    public string ImageUrl { get; set; }  // URL or path to the image
}
public class OrganizerDTO
{
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
}
