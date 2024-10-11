using AutoMapper;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Mappings
{
    public class AutoMappings: Profile
    {
        public AutoMappings()
        {
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<Event, AddNewEventDTO>().ReverseMap();
        }
    }
}
    