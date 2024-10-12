using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IMapper mapper;
        private readonly IEventRepository eventRepository;

        public BookingsController(IBookingRepository bookingRepository, IMapper mapper, IEventRepository eventRepository)
        {
            this.bookingRepository = bookingRepository;
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        // POST: BOOK AN EVENT BY ID
        [HttpPost]
        [Route("Book/{EventId}")]
        public async Task<IActionResult> BookEventById([FromRoute] Guid EventId, [FromBody] AddBookingDTO addBookingDTO)
        {
            var eventToBeBooked = await eventRepository.GetEvent(EventId);

            if (eventToBeBooked == null)
            {
                return NotFound();
            }

            var bookingDM = new Booking
            {
                BookingID = Guid.NewGuid(),
                EventID = eventToBeBooked.EventID,
                UserID = addBookingDTO.UserID,
                BookingDate = DateTime.Now,
                NumberOfTickets = addBookingDTO.NumberOfTickets,
                Amount = addBookingDTO.NumberOfTickets * eventToBeBooked.TicketPrice,
            };

            var bookingResult = await bookingRepository.BookEvent(bookingDM);

            // Check if bookingResult is null (error such as not enough tickets)
            if (bookingResult == null)
            {
                return BadRequest("Unable to book the event. Not enough tickets or event not found.");
            }

            var bookingDTO = mapper.Map<BookingDTO>(bookingResult);
            return CreatedAtAction(nameof(BookEventById), new { id = bookingDTO.EventID }, bookingDTO);
        }

        // GET : GET ALL BOOKED EVENTS 
        [HttpGet]
        [Route("All")]
        //[Authorize(Roles = "Event Manager")]
        public async Task<IActionResult> GetAllBookings()
        {
            var allBookings = await bookingRepository.GetAllBookings();

            var allBookingsDTO = mapper.Map<List<BookingDTO>>(allBookings); // Correct mapping
            return Ok(allBookingsDTO); // Return DTO, not domain model
        }

        // GET : GET ALL THE EVENTS BOOKED BY A USER
        [HttpGet]
        [Route("Bookings/{userId}")]
        public async Task<IActionResult> GetUserBookings([FromRoute] string userId)
        {
            var userBookings = await bookingRepository.GetUserBookings(userId);

            if (userBookings == null)
            {
                return NotFound($"No bookings found for user {userId}.");
            }

            var userBookingsDTO = mapper.Map<List<BookingDTO>>(userBookings);
            return Ok(userBookingsDTO);
        }

        // DELETE : DELETE A BOOKING
        [HttpDelete]
        [Route("Delete/{BookingId}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid BookingId)
        {
            var bookingToBeDeleted = await bookingRepository.DeleteBooking(BookingId);

            if (bookingToBeDeleted == null)
            {
                return NotFound($"Booking with ID {BookingId} not found.");
            }

            return Ok(mapper.Map<BookingDTO>(bookingToBeDeleted)); // Map deleted booking to DTO and return
        }
    }
}
