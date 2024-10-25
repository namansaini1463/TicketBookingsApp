using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(10)]
        [MinLength(10)]
        public string? PhoneNumber { get; set; }  // Optional phone number field

        public IFormFile? ProfilePicture { get; set; }  // Optional profile picture

        public string? ProfilePictureUrl { get; set; } // Optional profile picture

        public string? PreferredLanguage { get; set; }// Optional preferred language

        public string? PreferredCurrency { get; set; } // Optional preferred currency
    }
}