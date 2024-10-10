using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public class SQLBookingRepository : IBookingRepository
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;

        public SQLBookingRepository(TicketBookingsAppDBContext ticketBookingsAppDBContext)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
        }
        public async Task<List<Booking>> GetAllBookings()
        {
            return await ticketBookingsAppDBContext.Bookings
                                        .Include("BookingUser")
                                        .Include("BookingEvent")
                                        .ToListAsync();
        }

        public async Task<Booking> BookEvent(Booking bookingDM)
        {
            await ticketBookingsAppDBContext.Bookings.AddAsync(bookingDM);
            await ticketBookingsAppDBContext.SaveChangesAsync();

            var result = await ticketBookingsAppDBContext.Bookings.Include("BookingUser").Include("BookingEvent").FirstOrDefaultAsync(e => e.BookingID == bookingDM.BookingID);
            return result;

        }

        public async Task<Booking?> DeleteBooking(Guid bookingID)
        {
            var existingBooking =  await ticketBookingsAppDBContext.Bookings.FirstOrDefaultAsync(e => e.BookingID == bookingID);


            if (existingBooking == null)
            {
                return null;
            }

            //ticketBookingsAppDBContext.Bookings.Remove(existingBooking);
            //await ticketBookingsAppDBContext.SaveChangesAsync();

            await ticketBookingsAppDBContext.Bookings.Where(e => e.BookingID == bookingID).ExecuteDeleteAsync();
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return existingBooking;
        }

        //public async Task<List<Booking>> DeleteAllBookings(Guid bookingID)
        //{
        //    await ticketBookingsAppDBContext.Bookings.
        //}
    }
}
