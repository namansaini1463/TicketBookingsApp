namespace TicketBookingsAppAPI.Models.DTOs
{
    public class CartItemDTO
    {
        public string userID { get; set; }
        public Guid TicketTypeID { get; set; }
        public int Quantity { get; set; }
    }
}
