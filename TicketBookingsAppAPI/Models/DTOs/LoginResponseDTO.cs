using System.Reflection.Metadata.Ecma335;
using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        public UserProfile UserProfile { get; set; }
        public string JWTToken { get; set; }
        public string Username { get; set; }
    }

    public class UserProfile
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PreferredLanguage { get; set; }
        public string PreferredCurrency { get; set; }

    }
}
