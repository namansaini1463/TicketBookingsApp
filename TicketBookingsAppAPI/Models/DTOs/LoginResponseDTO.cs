namespace TicketBookingsAppAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        public string Name { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string JWTToken { get; set; }
    }
}
