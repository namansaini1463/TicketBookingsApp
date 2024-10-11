using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Mappings;
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
        private readonly IMapper mapper;

        public EventsController(TicketBookingsAppDBContext ticketBookingsAppDBContext, IEventRepository eventRepository, IMapper mapper)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        // GET ALL EVENTS
        // GET : https://localhost:7290/api/Events
        [HttpGet]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllEvents()
        {
            var allEvents = await eventRepository.GetAllEvents();

            var allEventsDTO = new List<EventDTO>();
            mapper.Map(allEvents, allEventsDTO);

            return Ok(allEventsDTO);
        }

        // GET EVENT BY ID
        // GET : https://localhost:7290/api/Events/{id}
        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetEvent([FromRoute] Guid id)
        {
            var eventByID = await eventRepository.GetEvent(id);

            if (eventByID == null)
            {
                return NotFound();
            }

            var eventByNameDTO = mapper.Map<EventDTO>(eventByID);

            return Ok(eventByNameDTO);
        }

        
        // POST A NEW EVENT
        // POST : https://localhost:7290/api/Events
        [HttpPost]
        //[Authorize(Roles = "Event Manager")]
        public async Task<IActionResult> AddEvent([FromBody] AddNewEventDTO addNewEventDTO)
        {
            var eventDM = mapper.Map<Event>(addNewEventDTO);

            eventDM = await eventRepository.AddEvent(eventDM);

            var eventDTO = mapper.Map<EventDTO>(eventDM);

            return CreatedAtAction(nameof(AddEvent), new {id = eventDTO.EventID}, eventDTO);
        }

        // DELETE AN EVENT BY ID
        // DELETE : https://localhost:7290/api/Events/{id}
        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Roles = "Event Manager")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedEventDM = await eventRepository.DeleteEvent(id);

            if(deletedEventDM == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<EventDTO>(deletedEventDM));

        }
    }
}
