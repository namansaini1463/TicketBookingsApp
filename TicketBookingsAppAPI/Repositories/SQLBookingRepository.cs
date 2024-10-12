using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public class SQLBookingRepository : IBookingRepository
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;
        private readonly IEventRepository eventRepository;

        public SQLBookingRepository(TicketBookingsAppDBContext ticketBookingsAppDBContext, IEventRepository eventRepository)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
            this.eventRepository = eventRepository;
        }
        public async Task<List<Booking>> GetAllBookings()
        {
            return await ticketBookingsAppDBContext.Bookings
                                        .Include(b => b.BookingUser) 
                                        .Include(b => b.BookingEvent)
                                        .ToListAsync();
        }

        public async Task<Booking> BookEvent(Booking bookingDM)
        {
            var eventData = await eventRepository.GetEvent(bookingDM.EventID);

            if (eventData == null)
            {
                return null; // Return null if event not found
            }

            if (bookingDM.NumberOfTickets > eventData.AvailableTickets)
            {
                return null; // Return null if not enough tickets
            }

            // Adjust available tickets for the event
            eventData.AvailableTickets -= bookingDM.NumberOfTickets;

            // Add the booking to the database
            await ticketBookingsAppDBContext.Bookings.AddAsync(bookingDM);
            await ticketBookingsAppDBContext.SaveChangesAsync();

            // Fetch the created booking with related entities
            var result = await ticketBookingsAppDBContext.Bookings
                            .Include(b => b.BookingUser)
                            .Include(b => b.BookingEvent)
                            .FirstOrDefaultAsync(b => b.BookingID == bookingDM.BookingID);

            return result;

        }

        public async Task<Booking?> DeleteBooking(Guid bookingID)
        {
            // Find the booking to be deleted
            var existingBooking = await ticketBookingsAppDBContext.Bookings
                                    .Include(b => b.BookingEvent) // Ensure the event is included
                                    .FirstOrDefaultAsync(b => b.BookingID == bookingID);

            if (existingBooking == null)
            {
                return null; // Return null if booking not found
            }

            // Fetch the associated event
            var eventData = await eventRepository.GetEvent(existingBooking.EventID);

            if (eventData != null)
            {
                // Increase the available tickets by the number of tickets in the booking
                eventData.AvailableTickets += existingBooking.NumberOfTickets;

                // Save the changes to the event
                ticketBookingsAppDBContext.Events.Update(eventData);
            }

            // Delete the booking
            await ticketBookingsAppDBContext.Bookings.Where(b => b.BookingID == bookingID).ExecuteDeleteAsync();
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return existingBooking; // Return the deleted booking
        }


        public async Task<Booking?> GetBookingById(Guid bookingID)
        {
            var booking = await ticketBookingsAppDBContext.Bookings
                                .Include(b => b.BookingUser)
                                .Include(b => b.BookingEvent)
                                .FirstOrDefaultAsync(b => b.BookingID == bookingID);

            return booking; // Return booking or null if not found
        }

        public async Task<List<Booking>?> GetUserBookings(string userID)
        {
            var bookings = await ticketBookingsAppDBContext.Bookings
                                 .Include(b => b.BookingUser)
                                 .Include(b => b.BookingEvent)
                                 .Where(b => b.BookingUser.Id == userID)
                                 .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return null; // Return null if no bookings found for the user
            }

            return bookings;
        }
    }
}
