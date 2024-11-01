using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;
using TicketBookingsAppAPI.Repositories;

namespace TicketBookingsAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddCoupon([FromBody] Coupon newCoupon)
        {
            if (newCoupon == null)
            {
                return BadRequest(new { message = "Invalid coupon data." });
            }

            // Set default values for new coupon
            newCoupon.CouponID = Guid.NewGuid();
            newCoupon.CurrentUses = 0;

            // Add the coupon to the database
            await couponRepository.AddCouponAsync(newCoupon);
            return Ok(new { message = "Coupon added successfully.", newCoupon });
        }
    }
}
