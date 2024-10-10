using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public class SQLEventRepository : IEventRepository
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;

        public SQLEventRepository(TicketBookingsAppDBContext ticketBookingsAppDBContext)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
        }

        public async Task<Event> AddEvent(Event addEvent)
        {
            await ticketBookingsAppDBContext.Events.AddAsync(addEvent);
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return addEvent;
        }

        public async Task<Event?> DeleteEvent(Guid EventID)
        {
            var existingEvent = await ticketBookingsAppDBContext.Events.FirstOrDefaultAsync(e => e.EventID == EventID);

            if (existingEvent == null) {
                return null;
            }

            ticketBookingsAppDBContext.Events.Remove(existingEvent);
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return existingEvent;   
        }

        public async Task<List<Event>> GetAllEvents()
        {
            return await ticketBookingsAppDBContext.Events.ToListAsync();
        }

        public async Task<Event?> GetEvent(Guid EventID)
        {
            return await ticketBookingsAppDBContext.Events.FirstOrDefaultAsync(e => e.EventID == EventID);

        }
    }
}
