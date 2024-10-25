using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class EventImage
    {
        [Key]
        public Guid EventImageID { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public long SizeInBytes { get; set; }

        // Foreign key for Event
        [Required]
        public Guid EventID { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; }
    }
}
