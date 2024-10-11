using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.DTOs
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        public string[] Roles { get; set; }
    }
}
