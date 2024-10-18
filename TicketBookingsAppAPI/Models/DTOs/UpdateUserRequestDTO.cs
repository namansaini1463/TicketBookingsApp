using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(10, ErrorMessage = "Error here in updaterDTO")]
        [MinLength(10, ErrorMessage = "Error here in updaterDTO")]
        public string? PhoneNumber { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? PreferredCurrency { get; set; }
        public string? OldPassword { get; set; }

        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long.")]
        public string? NewPassword { get; set; }
    }

}
