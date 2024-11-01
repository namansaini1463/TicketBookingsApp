using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Repositories
{
    public interface ICouponRepository
    {
        Task AddCouponAsync(Coupon coupon);
        Task<Coupon> GetCouponByCodeAsync(string code); 
        Task<bool> ValidateCouponAsync(string code);
        Task ApplyCouponAsync(Coupon coupon);
    }
}
