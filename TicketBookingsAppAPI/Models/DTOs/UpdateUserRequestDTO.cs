using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string UserID { get; set; }
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // Regular expression to validate a 10-digit phone number or allow null/empty
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? PhoneNumber { get; set; }  // Optional phone number field

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? PreferredLanguage { get; set; }
        public string? PreferredCurrency { get; set; }
        public string? OldPassword { get; set; }

        [MinLength(8, ErrorMessage = "New password must be at least 8 characters long.")]
        public string? NewPassword { get; set; }
    }
}
