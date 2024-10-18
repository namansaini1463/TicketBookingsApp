using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string EmailOrUsername { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
    }
}
