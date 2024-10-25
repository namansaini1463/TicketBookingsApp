using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;
        private readonly IEventRepository eventRepository;
        private readonly IEventRepository eventRepository1;
        private readonly IMapper mapper;

        public EventsController(TicketBookingsAppDBContext ticketBookingsAppDBContext, IEventRepository eventRepository, IMapper mapper)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent([FromBody] EventCreateDTO createEventDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return 400 Bad Request if validation fails
            }

            // Map the DTO to the domain model (Event)
            var newEventDM = new Event
            {
                Name = createEventDTO.Name,
                DateAndTime = createEventDTO.DateAndTime,
                Venue = new Venue
                {
                    Name = createEventDTO.Venue.Name,
                    Address = createEventDTO.Venue.Address,
                    State = createEventDTO.Venue.State,
                    Capacity = createEventDTO.Venue.Capacity
                },
                EventDescription = createEventDTO.EventDescription,
                Categories = createEventDTO.Categories,  
                TicketTypes = createEventDTO.TicketTypes.Select(t => new TicketType
                {
                    Type = t.Type,
                    Price = t.Price,
                    QuantityAvailable = t.QuantityAvailable,
                }).ToList(),
                Images = createEventDTO.Images.Select(i => new EventImage
                {
                    ImageUrl = i.ImageUrl
                }).ToList(),
                Organizer = new Organizer
                {
                    Name = createEventDTO.Organizer.Name,
                    ContactEmail = createEventDTO.Organizer.ContactEmail,
                    PhoneNumber = createEventDTO.Organizer.PhoneNumber
                }
            };

            // Save the event to the database
            newEventDM = await eventRepository.PostEvent(newEventDM);

            var newEventDTO = mapper.Map<EventDTO>(newEventDM);

            // Return the created event
            return CreatedAtAction(nameof(GetEvent), new { id = newEventDTO.EventID }, newEventDTO);
        }

        // GET: api/Events/id
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(Guid id)
        {
            var eventDM = await eventRepository.GetEvent(id);

            if (eventDM == null)
            {
                return NotFound();
            }

            var eventDTO = mapper.Map<EventDTO>(eventDM);

            return Ok(eventDTO);
        }

        // GET : api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null,
            [FromQuery] EventCategory? eventCategory = null
        )
        {
            var filterParameters = new EventFilterParametersDTO
            {
                searchTerm = searchTerm,
                sortBy = sortBy,
                sortOrder = sortOrder,
                StartDate = startDate,
                EndDate = endDate,
                Category = eventCategory
            };

            var allEventsDM = await eventRepository.GetAllEvents(filterParameters);

            if (allEventsDM == null)
            {
                return NotFound();  // Return 404 if no events are found
            }

            var allEventsDTO = mapper.Map<IEnumerable<EventDTO>>(allEventsDM);

            return Ok(allEventsDTO);
        }
    }
}
