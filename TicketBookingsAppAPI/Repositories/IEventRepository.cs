using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEvents(EventFilterParametersDTO filterParametersDTO);
        Task<Event?> GetEvent(Guid id);
        Task<Event> PostEvent(Event addEvent);
    }
}
