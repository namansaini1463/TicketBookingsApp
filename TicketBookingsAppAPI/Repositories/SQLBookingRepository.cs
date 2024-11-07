using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Data;
using TicketBookingsAppAPI.Models.Domain;
using TicketBookingsAppAPI.Models.DTOs;

namespace TicketBookingsAppAPI.Repositories
{
    public class SQLBookingRepository : IBookingRepository
    {
        private readonly TicketBookingsAppDBContext ticketBookingsAppDBContext;
        private readonly ICouponRepository couponRepository;

        public SQLBookingRepository(TicketBookingsAppDBContext ticketBookingsAppDBContext, ICouponRepository couponRepository)
        {
            this.ticketBookingsAppDBContext = ticketBookingsAppDBContext;
            this.couponRepository = couponRepository;
        }

        public async Task CancelBookingAsync(Guid bookingId)
        {
            var booking = await ticketBookingsAppDBContext.Bookings
                .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.TicketType)
                .FirstOrDefaultAsync(b => b.BookingID == bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            // Check if the booking is already cancelled
            if (booking.BookingStatus == BookingStatus.Cancelled)
            {
                throw new Exception("Booking is already cancelled.");
            }

            // Restore the ticket quantities for each booking item
            foreach (var bookingItem in booking.BookingItems)
            {
                var ticketType = await ticketBookingsAppDBContext.TicketTypes.FindAsync(bookingItem.TicketTypeID);
                if (ticketType != null)
                {
                    ticketType.QuantityAvailable += bookingItem.Quantity; // Restore the available quantity
                }
            }

            // Update the booking status to Cancelled
            booking.BookingStatus = BookingStatus.Cancelled;

            // Update the payment status to Refunded
            var payment = await ticketBookingsAppDBContext.Payments.FirstOrDefaultAsync(p => p.BookingID == bookingId);
            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Refunded;
                payment.RefundDate = DateTime.UtcNow; // Optional: Track the refund date
            }

            // Save the changes
            await ticketBookingsAppDBContext.SaveChangesAsync();
        }

        public async Task<Booking> CreateBookingFromCartAsync(string userId, PaymentDTO paymentDTO, string couponCode = null)
        {
            // Fetch the user's cart with items
            var cart = await ticketBookingsAppDBContext.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.TicketType)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty or does not exist.");
            }

            decimal totalAmount = cart.CartItems.Sum(ci => ci.TicketType.Price * ci.Quantity);
            decimal discountAmount = 0;
            Coupon coupon = null;

            // Validate and apply coupon if provided
            if (!string.IsNullOrEmpty(couponCode))
            {
                coupon = await couponRepository.GetCouponByCodeAsync(couponCode);
                if (coupon == null || !await couponRepository.ValidateCouponAsync(couponCode))
                {
                    throw new Exception("Invalid or expired coupon code.");
                }

                // Calculate discount
                discountAmount = coupon.IsPercentage ? (totalAmount * (coupon.DiscountValue / 100)) : coupon.DiscountValue;
                totalAmount -= discountAmount; // Apply discount to total amount
            }

            // Determine booking status based on payment status
            var bookingStatus = paymentDTO.paymentStatus == PaymentStatus.Paid ? BookingStatus.Confirmed : BookingStatus.Pending;

            // Create a new booking
            var booking = new Booking
            {
                BookingID = Guid.NewGuid(),
                UserID = userId,
                BookingDate = DateTime.UtcNow,
                BookingStatus = bookingStatus,
                TotalAmount = totalAmount,
                DiscountApplied = discountAmount,
                CouponID = coupon?.CouponID
            };

            // Add booking items from the cart
            foreach (var cartItem in cart.CartItems)
            {
                var ticketType = cartItem.TicketType;
                if (ticketType.QuantityAvailable < cartItem.Quantity)
                {
                    throw new Exception($"Not enough tickets available for {ticketType.Type}.");
                }

                // Update available tickets only if the booking is confirmed
                if (bookingStatus == BookingStatus.Confirmed)
                {
                    ticketType.QuantityAvailable -= cartItem.Quantity;
                }

                var bookingItem = new BookingItem
                {
                    BookingItemID = Guid.NewGuid(),
                    BookingID = booking.BookingID,
                    TicketTypeID = cartItem.TicketTypeID,
                    Quantity = cartItem.Quantity
                };

                booking.BookingItems.Add(bookingItem);
            }

            // Add the booking to the database
            await ticketBookingsAppDBContext.Bookings.AddAsync(booking);

            // Remove cart items and clear the cart
            ticketBookingsAppDBContext.CartItems.RemoveRange(cart.CartItems);

            // Update coupon usage if applied
            if (coupon != null)
            {
                await couponRepository.ApplyCouponAsync(coupon);
            }

            // Create and add the payment details for the booking
            var payment = new Payment
            {
                PaymentID = Guid.NewGuid(),
                BookingID = booking.BookingID,
                Amount = totalAmount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = paymentDTO.paymentMethod,
                TransactionID = paymentDTO.transactionId ?? Guid.NewGuid(),
                PaymentStatus = paymentDTO.paymentStatus // Set based on passed parameter
            };
            await ticketBookingsAppDBContext.Payments.AddAsync(payment);

            // Save all changes to the database
            await ticketBookingsAppDBContext.SaveChangesAsync();

            return booking;
        }


        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            return await ticketBookingsAppDBContext.Bookings
                .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.TicketType)
                .ThenInclude(bi => bi.Event)
                .ThenInclude(bi => bi.Images)
                .FirstOrDefaultAsync(b => b.BookingID == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByUserIdAsync(string userId)
        {
            {
                return await ticketBookingsAppDBContext.Bookings
                    .Include(b => b.BookingItems) // Include associated booking items
                    .ThenInclude(bi => bi.TicketType) // Include associated ticket types for each booking item
                    .ThenInclude(bi => bi.Event)
                    .ThenInclude(bi => bi.Images)
                    .Where(b => b.UserID == userId)
                    .ToListAsync();
            }
        }
    }
}
