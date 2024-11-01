using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        public async Task<List<Event>> GetAllEvents(EventFilterParametersDTO filterParametersDTO)
        {
            IQueryable<Event> query = ticketBookingsAppDBContext.Events
                .Include(e => e.Venue)
                .Include(e => e.Organizer)
                .Include(e => e.TicketTypes)
                .Include(e => e.Images)
                .AsQueryable();

            // Apply filtering
            if (!string.IsNullOrEmpty(filterParametersDTO.searchTerm))
            {
                string searchTerm = filterParametersDTO.searchTerm.ToLower();

                query = query.Where(e =>
                    e.Name.ToLower().Contains(searchTerm) ||      // Match by event name
                    e.Venue.State.ToLower().Contains(searchTerm) || // Match by venue state
                    e.Venue.Name.ToLower().Contains(searchTerm) || // Match by venue state
                    e.Venue.Address.ToLower().Contains(searchTerm) || // Match by venue state
                    e.Organizer.Name.ToLower().Contains(searchTerm) ||// Match by organizer name
                    e.EventDescription.ToLower().Contains(searchTerm) 

                );
            }

            // Date filtering
            if (filterParametersDTO.StartDate.HasValue && filterParametersDTO.EndDate.HasValue)
            {
                query = query.Where(e => e.DateAndTime >= filterParametersDTO.StartDate.Value &&
                                         e.DateAndTime <= filterParametersDTO.EndDate.Value);
            }

            // Apply filtering by category
            if (filterParametersDTO.Category.HasValue)
            {
                query = query.Where(e => e.Categories.Contains(filterParametersDTO.Category.Value));
            }

            // Sorting
            string sortOrder = filterParametersDTO.sortOrder?.ToLower() ?? "asc"; // Default to 'asc' if sortOrder is null

            switch (filterParametersDTO.sortBy?.ToLower())
            {
                case "name":
                    query = sortOrder == "desc" ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
                    break;
                case "venue":
                    query = sortOrder == "desc" ? query.OrderByDescending(e => e.Venue.Name) : query.OrderBy(e => e.Venue.Name);
                    break;
                case "price":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(e => e.TicketTypes.Min(t => t.Price)) // Sort by minimum price, descending
                        : query.OrderBy(e => e.TicketTypes.Min(t => t.Price)); // Sort by minimum price, ascending
                    break;
                case "date":
                    query = sortOrder == "desc" ? query.OrderByDescending(e => e.DateAndTime) : query.OrderBy(e => e.DateAndTime);
                    break;
                default:
                    query = sortOrder == "desc" ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name); // Default sorting by name
                    break;
            }

            return await query.ToListAsync();
        }


        public async Task<Event?> GetEvent(Guid id)
        {
            var eventDM = await ticketBookingsAppDBContext.Events
                                             .Include(e => e.TicketTypes)
                                             .Include(e => e.Images)
                                             .FirstOrDefaultAsync(e => e.EventID == id);
            if (eventDM == null)
            {
                return null;
            }

            return eventDM;
        }

        public async Task<Event> PostEvent(Event addEvent)
        {
            await ticketBookingsAppDBContext.Events.AddAsync(addEvent);
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return addEvent;
        }
    }
}
