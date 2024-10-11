using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;
        private readonly IBookingRepository bookingRepository;
        private readonly IMapper mapper;

        public BookingsController(TicketBookingsAppDBContext ticketBookingsAppDBContext, IBookingRepository bookingRepository, IMapper mapper)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
            this.bookingRepository = bookingRepository;
            this.mapper = mapper;
        }

        // POST: BOOK AN EVENT BY NAME
        //[HttpPost]
        //[Route("Book/{name}")]
        //public async Task<IActionResult> BookEventByName([FromRoute] string name, [FromBody] AddBookingDTO addBookingDTO)
        //{
        //    var eventToBeBooked = await ticketBookingsAppDBContext.Events.FirstOrDefaultAsync(e => e.Name == name);

        //    if (eventToBeBooked == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookingDM = new Booking
        //    {
        //        BookingID = Guid.NewGuid(),
        //        EventID = eventToBeBooked.EventID,
        //        UserID = addBookingDTO.UserID,
        //        BookingDate = DateTime.Now,
        //        NumberOfTickets = addBookingDTO.NumberOfTickets,
        //        Amount = addBookingDTO.NumberOfTickets * eventToBeBooked.TicketPrice,
        //    };

        //    await bookingRepository.BookEvent(bookingDM);

        //    return CreatedAtAction(nameof(BookEventByName), new { id = bookingDM.EventID }, bookingDM);


        //}

        // POST: BOOK AN EVENT BY ID
        
        [HttpPost]
        [Route("Book/{EventId}")]
        public async Task<IActionResult> BookEventById([FromRoute] Guid EventId, [FromBody] AddBookingDTO addBookingDTO)
        {
            var eventToBeBooked = await ticketBookingsAppDBContext.Events.FirstOrDefaultAsync(e => e.EventID == EventId);

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

            await bookingRepository.BookEvent(bookingDM);

            return CreatedAtAction(nameof(BookEventById), new { id = bookingDM.EventID }, bookingDM);
        }

        // GET : GET ALL BOOKED EVENTS 
        [HttpGet]
        [Route("All")]
        //[Authorize(Roles = "Event Manager")]
        public async Task<IActionResult> GetAllBookings()
        {
            var allBookings = await bookingRepository.GetAllBookings();

            var allBookingsDTO = new List<BookingDTO>();
            mapper.Map(allBookingsDTO, allBookingsDTO);

            return Ok(allBookings);

        }

        // DELETE : DELETE A BOOKING
        [HttpDelete]
        [Route("Delete/{BookingId}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid BookingId)
        {
            var bookingToBeDeleted = await bookingRepository.DeleteBooking(BookingId);

            if(bookingToBeDeleted == null)
            {
                return NotFound();
            }

            return Ok(bookingToBeDeleted);
        }
    }
}
