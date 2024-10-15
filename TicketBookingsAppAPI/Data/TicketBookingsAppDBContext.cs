using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Models.Domain;
using System.Collections.Generic;

namespace TicketBookingsAppAPI.Data
{
    public class TicketBookingsAppDBContext : IdentityDbContext<User>
    {
        public TicketBookingsAppDBContext(DbContextOptions<TicketBookingsAppDBContext> options)
            : base(options) { }

        // Define your DbSets for the application models
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<EventImage> EventImages { get; set; }

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
        }
    }
}
