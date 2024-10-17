using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class User : IdentityUser
    {
        // UserID -> Identity User

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }


        // Phone Number -> Identity User 
        // Password -> Identity User

        public string ProfilePictureUrl { get; set; }
        public string PreferredLanguage { get; set; }
        public string PreferredCurrency { get; set; }

    }
}
