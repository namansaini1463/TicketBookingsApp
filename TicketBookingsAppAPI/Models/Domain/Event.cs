using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class Event
    {
        [Key]  // Primary Key
        public Guid EventID { get; set; }

        [Required]
        [MaxLength(100)]  // Restrict the name to 100 characters
        public string Name { get; set; }

        [Required]
        public DateTimeOffset DateAndTime { get; set; }

        // Embedded Venue information
        [Required]
        public Venue Venue { get; set; }

        // Collection of TicketTypes with FK to Event
        public List<TicketType> TicketTypes { get; set; } = new List<TicketType>();

        [MaxLength(2000)]  // Restrict event description to 2000 characters
        public string? EventDescription { get; set; }

        // Categories (multi-select)
        [Required]
        public List<EventCategory> Categories { get; set; } = new List<EventCategory>();

        // Calculated field for available tickets (not mapped to DB)
        [NotMapped]  // Do not map this to the database
        public int AvailableTickets
        {
            get
            {
                return TicketTypes.Sum(ticket => ticket.QuantityAvailable);
            }
        }

        // Event Images with FK to Event
        public List<EventImage> Images { get; set; } = new List<EventImage>();

        // Embedded Organizer information
        [Required]
        public Organizer Organizer { get; set; }

        //public int TicketPrice { get; internal set; }
    }

    // Venue Class for detailed venue information (embedded)
    public class Venue
    {
        [Required]
        [MaxLength(255)]  // Venue name length
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]  // Venue address length
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]  // State length
        public string State { get; set; }

        [Required]
        public int Capacity { get; set; }
    }    

    // Organizer Class (embedded)
    public class Organizer
    {
        [Required]
        [MaxLength(255)]  // Organizer name length
        public string Name { get; set; }

        [Required]
        [EmailAddress]  // Enforce valid email
        public string ContactEmail { get; set; }

        [Phone]  // Enforce valid phone number
        public string PhoneNumber { get; set; }
    }

    // Enum for Event Categories
    public enum EventCategory
    {
        Music,
        Sports,
        Technology,
        Education,
        Business,
        Arts,
        Health,
        Entertainment,
        FoodAndDrink,
        Fashion,
        Charity,
        Comedy,
        FilmAndTheater,
        Literature,
        Networking,
        Science,
        TravelAndAdventure,
        Workshop,
        Conference,
        Festival,
        Religious,
        Fitness,
        Family,
        Gaming,
        History,
        Politics,
        Spirituality,
        Environment,
        Photography,
        PetsAndAnimals
    }

}
