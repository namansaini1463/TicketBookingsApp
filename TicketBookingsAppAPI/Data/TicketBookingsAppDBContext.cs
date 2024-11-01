using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Data
{
    public class TicketBookingsAppDBContext : IdentityDbContext<User>
    {
        public TicketBookingsAppDBContext(DbContextOptions<TicketBookingsAppDBContext> options)
            : base(options) { }

        // Defining the DbSets for the application models
        public DbSet<Event> Events { get; set; }
        public DbSet<EventImage> EventImages { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles into the database
            var eventManagerRoleId = "35fab44a-d306-489a-8287-dec476bbeb1f";
            var userRoleId = "7390edf6-3816-4067-84b3-2f86d3072b8b";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = eventManagerRoleId,
                    ConcurrencyStamp = eventManagerRoleId,
                    Name = "Event Manager",
                    NormalizedName = "EVENT MANAGER"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            // Apply role seeding
            builder.Entity<IdentityRole>().HasData(roles);

            // Embed Venue as a value object in the Event
            builder.Entity<Event>()
                .OwnsOne(e => e.Venue);

            // Embed Organizer as a value object in the Event
            builder.Entity<Event>()
                .OwnsOne(e => e.Organizer);

            // Configure relationships between Event and TicketType
            builder.Entity<Event>()
                .HasMany(e => e.TicketTypes)
                .WithOne(t => t.Event)  // Use the correct navigation property in TicketType
                .HasForeignKey(t => t.EventID);  // Set explicit foreign key property

            // Configure relationships between Event and EventImage
            builder.Entity<Event>()
                .HasMany(e => e.Images)
                .WithOne(i => i.Event)  // Use the correct navigation property in EventImage
                .HasForeignKey(i => i.EventID);  // Set explicit foreign key property

            // Configure relationships between User and Shopping Cart
            builder.Entity<User>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(sc => sc.User)
                .HasForeignKey<ShoppingCart>(sc => sc.UserID); // Use ShoppingCart's UserID as foreign key

            // Configure relationships between User and Bookings
            builder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User) // Assuming Booking has a navigation property `User`
                .HasForeignKey(b => b.UserID); // Set Booking's UserID as foreign key

            // Configure relationships between Booking and BookingItem
            builder.Entity<Booking>()
                .HasMany(b => b.BookingItems)
                .WithOne(i => i.Booking) // Assuming BookingItem has a navigation property `Booking`
                .HasForeignKey(i => i.BookingID); // Use BookingItem's BookingID as foreign key

            // Configure the foreign key relationship between Booking and Coupon
            builder.Entity<Booking>()
               .HasOne(b => b.Coupon)
               .WithMany()
               .HasForeignKey(b => b.CouponID)
               .OnDelete(DeleteBehavior.SetNull); // Allow null values if no coupon is used
        }
    }
}
