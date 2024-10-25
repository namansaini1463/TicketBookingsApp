namespace TicketBookingsAppAPI.Models.DTOs
{
    public class UpdateCartItemDTO
    {
        public Guid CartItemID { get; set; }
        public int Quantity { get; set; }
    }
}
