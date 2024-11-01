using System.ComponentModel.DataAnnotations;

namespace TicketBookingsAppAPI.Models.Domain
{
    public class Coupon
    {
        public Guid CouponID { get; set; }
        public string Code { get; set; }
        public bool IsPercentage { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime ExpiryDate { get; set; } // Expiration date for the coupon
        public int CurrentUses { get; set; } = 0; // Track current usage count
        public int MaxUses { get; set; } // Maximum number of uses allowed

    }
}
