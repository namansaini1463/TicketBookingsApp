using System.Runtime.CompilerServices;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingFromCartAsync(string userId, PaymentDTO paymentDTO, string couponCode);
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
        Task<List<Booking>> GetBookingsByUserIdAsync(string userId);
        Task CancelBookingAsync(Guid bookingId);
    }
}
