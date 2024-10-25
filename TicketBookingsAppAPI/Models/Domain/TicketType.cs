using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class TicketType
    {
        [Key]
        public Guid TicketTypeID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } // Ticket type (e.g., VIP, General Admission)

        [Required]
        [Column(TypeName = "decimal(18,2)")]  // Specify decimal precision
        public decimal Price { get; set; }

        [Required]
        public int QuantityAvailable { get; set; }

        // Foreign key for Event
        [Required]
        public Guid EventID { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; }
    }
}
