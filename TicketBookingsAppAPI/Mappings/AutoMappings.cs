using AutoMapper;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Mappings
{
    public class AutoMappings: Profile
    {
        public AutoMappings()
        {
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<AddBookingDTO, Booking>().ReverseMap();

            // Event <-> EventDTO
            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.TicketTypes.Sum(t => t.QuantityAvailable)))
                .ForMember(dest => dest.TotalTicketPrice, opt => opt.MapFrom(src => src.TicketTypes.Sum(t => t.Price * t.QuantityAvailable)))
                .ReverseMap();


            // TicketType <-> TicketTypeDTO 
            CreateMap<TicketType, TicketTypeDTO>().ReverseMap();

            // Venue <-> VenueDTO
            CreateMap<Venue, VenueDTO>().ReverseMap();

            // EventImage <-> EventImageDTO
            CreateMap<EventImage, EventImageDTO>().ReverseMap();

            // Organizer <-> OrganizerDTO
            CreateMap<Organizer, OrganizerDTO>().ReverseMap();

        }
    }
}
    