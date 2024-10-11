using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketBookingsAppAPI.Models.Domain;

namespace TicketBookingsAppAPI.Data
{
    public class TicketBookingsAppDBContext: IdentityDbContext<User>
    {
        public TicketBookingsAppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var eventManagerRoleId = "35fab44a-d306-489a-8287-dec476bbeb1f";
            var userRoleId = "7390edf6-3816-4067-84b3-2f86d3072b8b";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = eventManagerRoleId,
                    ConcurrencyStamp = eventManagerRoleId,
                    Name = "Event Manager",
                    NormalizedName = "Event Manager".ToUpper()
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }


}
