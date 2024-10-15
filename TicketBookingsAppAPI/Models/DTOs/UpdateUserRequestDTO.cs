namespace TicketBookingsAppAPI.Models.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string UserID { get; set; } 
        public string Username { get; set; } 
        public string OldPassword { get; set; } 
        public string NewPassword { get; set; } 
    }
}
