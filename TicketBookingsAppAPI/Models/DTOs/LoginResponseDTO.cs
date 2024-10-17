namespace TicketBookingsAppAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        public string UserID { get; set; }
        public string JWTToken { get; set; }
    }
}
