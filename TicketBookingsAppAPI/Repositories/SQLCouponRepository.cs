using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Repositories
{
    public class SQLCouponRepository : ICouponRepository
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;

        public SQLCouponRepository(TicketBookingsAppDBContext ticketBookingsAppDBContext)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
        }

        public async Task<Coupon> GetCouponByCodeAsync(string code)
        {
            return await ticketBookingsAppDBContext.Coupons.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<bool> ValidateCouponAsync(string code)
        {
            Coupon coupon = await ticketBookingsAppDBContext.Coupons.FirstOrDefaultAsync(c => c.Code == code);

            if (coupon == null)
            {
                return false;
            }

            bool isNotExpired = coupon.ExpiryDate > DateTime.UtcNow;
            bool hasUsesLeft = coupon.CurrentUses < coupon.MaxUses;

            return isNotExpired && hasUsesLeft;
        }

        public async Task ApplyCouponAsync(Coupon coupon)
        {
            if (coupon.CurrentUses >= coupon.MaxUses)
            {
                throw new InvalidOperationException("Coupon has reached its maximum usage limit.");
            }

            coupon.CurrentUses += 1;
            ticketBookingsAppDBContext.Coupons.Update(coupon);
            await ticketBookingsAppDBContext.SaveChangesAsync();
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            await ticketBookingsAppDBContext.Coupons.AddAsync(coupon);
            await ticketBookingsAppDBContext.SaveChangesAsync();
        }
    }
}
