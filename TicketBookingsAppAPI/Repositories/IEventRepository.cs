using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEvents();
        Task<Event?> GetEvent(Guid id);
        Task<Event> PostEvent(Event addEvent);

        //Task<Event?> DeleteEvent(Guid id);
        //Task<Event?> UpdateEvent(Guid id);
    }
}
