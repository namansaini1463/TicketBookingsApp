using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository bookingRepository;

        public BookingsController(IBookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }
        // POST: api/Booking/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
        {
    
            try
            {
                var paymentDetails = new PaymentDTO
                {
                    paymentMethod = createBookingDTO.paymentMethod,
                    paymentStatus = createBookingDTO.paymentStatus,
                    transactionId = createBookingDTO.transactionId
                };
                var booking = await bookingRepository.CreateBookingFromCartAsync(createBookingDTO.UserID, paymentDetails, createBookingDTO.CouponCode);
                return Ok(new { Message = "Booking created successfully", Booking = booking });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("Cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            try
            {
                await bookingRepository.CancelBookingAsync(bookingId);
                return Ok(new { Message = "Booking cancelled successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBooking(Guid bookingId)
        {
            var booking = await bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return NotFound(new { Message = "Booking not found." });
            }

            return Ok(booking);
        }

        // GET: api/Bookings/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetUserBookings([FromRoute] string userId)
        {
            var bookings = await bookingRepository.GetBookingsByUserIdAsync(userId);

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound(new { Message = "No bookings found for this user." });
            }

            return Ok(bookings);
        }
    }
}
