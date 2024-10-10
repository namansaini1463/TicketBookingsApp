using System.Runtime.CompilerServices;
using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Repositories
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllBookings();
        Task<Booking> BookEvent(Booking bookingDM);

        Task<Booking?> DeleteBooking(Guid bookingID);

        //Task<List<Booking>> DeleteAllBookings(Guid bookingID);
    }
}
